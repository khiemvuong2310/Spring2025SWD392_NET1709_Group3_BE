using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DianaShop.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DianaShop.Service.RequestModel;
using System.Security.Cryptography;
using DianaShop.Service.Interfaces;
using DianaShop.Repository.Utils;
using System.Text.RegularExpressions;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DianaShop.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVoucherStorageService _voucherStorageService; // Inject VoucherService
        private readonly IImageService _imageService; // Inject ImageService
        private readonly IBlobService _blobService; // Inject BlobService

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IVoucherStorageService voucherStorageService, 
                          IImageService imageService, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _voucherStorageService = voucherStorageService;
            _imageService = imageService;
            _blobService = blobService;
        }

        // Lấy tất cả người dùng
        public async Task<IEnumerable<UserResponseModel>> GetAllUsersAsync()
        {
            return await _unitOfWork.Repository<User>().AsQueryable()
                .Where(u => !u.IsDelete)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // Lấy người dùng theo ID
        public async Task<UserResponseModel?> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.Repository<User>().AsQueryable()
                .Where(u => u.Id == id && !u.IsDelete)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        // Tạo người dùng mới
        public async Task<UserResponseModel> CreateUserAsync(UserCreateRequest request)
        {
            if (!Regex.IsMatch(request.Phone, "^\\+?[0-9]{10,15}$"))
            {
                throw new ArgumentException("Invalid phone number format.");
            }

            var existingUser = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var user = _mapper.Map<User>(request);
            user.Password = PasswordTools.HashPassword(user.Password);
            user.EmailVerified = true;
            
            // Upload image if provided
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var imageUrl = await _blobService.UploadImageAsync(request.ImageFile);
                user.Image = imageUrl;
            }

            // Lưu user vào database trước khi gán UserRoles để tránh lỗi vòng lặp
            await _unitOfWork.Repository<User>().InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Gán quyền mặc định (tránh ánh xạ trực tiếp trong UserCreateRequest)
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = 3,
                Status = true
            };
            await _unitOfWork.Repository<UserRole>().InsertAsync(userRole);
            await _unitOfWork.SaveChangesAsync();

            //Gọi phương thức tạo voucher cho user
            await _voucherStorageService.CreateVoucherStorageAsync(new VoucherStorageRequestModel
            {
                UserID = user.Id
            });

            var createdUser = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Id == user.Id)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (createdUser == null)
            {
                throw new Exception("Failed to retrieve created user.");
            }

            return createdUser;
        }

        public async Task<UserResponseModel?> UpdateUserAsync(int id, UserUpdateRequest request)
        {
            var existingUser = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Id == id && !u.IsDelete)
                .FirstOrDefaultAsync();

            if (existingUser == null) return null;

            // Kiểm tra xem có bất kỳ trường nào được cập nhật không
            bool hasUpdates = false;

            if (!string.IsNullOrEmpty(request.Email))
            {
                var emailUser = await _unitOfWork.Repository<User>()
                    .FindAsync(u => u.Email == request.Email && u.Id != id);

                if (emailUser != null)
                {
                    throw new InvalidOperationException("Email already exists.");
                }
                existingUser.Email = request.Email;
                hasUpdates = true;
            }

            // Cập nhật từng trường nếu có giá trị mới
            if (!string.IsNullOrEmpty(request.Username)) 
            {
                existingUser.Username = request.Username;
                hasUpdates = true;
            }
            
            // Xử lý hình ảnh
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingUser.Image))
                {
                    try
                    {
                        var fileName = Path.GetFileName(new Uri(existingUser.Image).LocalPath);
                        await _blobService.DeleteImageAsync(fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting old image: {ex.Message}");
                    }
                }

                // Upload ảnh mới
                var imageUrl = await _blobService.UploadImageAsync(request.ImageFile);
                existingUser.Image = imageUrl;
                hasUpdates = true;
                Console.WriteLine($"Updated image to: {imageUrl}");
            }
            else if (request.Image != null) 
            {
                existingUser.Image = request.Image;
                hasUpdates = true;
                Console.WriteLine($"Updated image URL to: {request.Image}");
            }
            
            if (!string.IsNullOrEmpty(request.Phone)) 
            {
                existingUser.Phone = request.Phone;
                hasUpdates = true;
            }
            if (!string.IsNullOrEmpty(request.Location)) 
            {
                existingUser.Location = request.Location;
                hasUpdates = true;
            }
            if (request.Status.HasValue) 
            {
                existingUser.Status = request.Status.Value;
                hasUpdates = true;
            }
            if (request.SkinTypeId.HasValue) 
            {
                existingUser.SkinTypeId = request.SkinTypeId.Value;
                hasUpdates = true;
            }

            if (hasUpdates)
            {
                await _unitOfWork.Repository<User>().Update(existingUser, id);
                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<UserResponseModel>(existingUser);
        }

        // Thêm/cập nhật hình ảnh hồ sơ
        public async Task<UserResponseModel?> AddProfileImageAsync(int id, IFormFile imageFile)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user == null || imageFile == null || imageFile.Length <= 0)
            {
                return null;
            }

            // Xóa ảnh cũ nếu có
            if (!string.IsNullOrEmpty(user.Image))
            {
                try
                {
                    var fileName = Path.GetFileName(new Uri(user.Image).LocalPath);
                    await _blobService.DeleteImageAsync(fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting old image: {ex.Message}");
                }
            }

            // Upload ảnh mới
            var imageUrl = await _blobService.UploadImageAsync(imageFile);
            user.Image = imageUrl;

            await _unitOfWork.Repository<User>().Update(user, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserResponseModel>(user);
        }

        // Cập nhật mật khẩu người dùng
        public async Task<bool> UpdatePasswordAsync(int id, string oldPassword, string newPassword)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!PasswordTools.VerifyPassword(oldPassword, user.Password))
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }

            if (newPassword.Length < 8 ||
                !Regex.IsMatch(newPassword, @"[A-Z]") ||
                !Regex.IsMatch(newPassword, @"\d"))
            {
                throw new ArgumentException("New password must be at least 8 characters long and contain at least one uppercase letter and one number.");
            }

            user.Password = PasswordTools.HashPassword(newPassword);
            await _unitOfWork.Repository<User>().Update(user, id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Cập nhật ví người dùng
        public async Task<bool> UpdateWalletAsync(int id, decimal? wallet)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user == null)
            {
                return false;
            }
            user.wallet = wallet;
            await _unitOfWork.Repository<User>().Update(user, id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Xóa người dùng
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user == null) return false;

            user.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Lấy người dùng theo username
        public async Task<UserResponseModel?> GetUserByUsernameAsync(string username)
        {
            return await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Username == username && !u.IsDelete)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        // Lấy người dùng theo email
        public async Task<UserResponseModel?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Email == email)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return user;
        }
    }
}
