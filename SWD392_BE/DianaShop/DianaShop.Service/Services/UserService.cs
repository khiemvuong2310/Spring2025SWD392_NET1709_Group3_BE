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
namespace DianaShop.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVoucherStorageService _voucherStorageService; // Inject VoucherService

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IVoucherStorageService voucherStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _voucherStorageService = voucherStorageService;
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
            var existingUserResponse = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Id == id && !u.IsDelete)
                .FirstOrDefaultAsync();

            if (existingUserResponse == null) return null;

            //var existingUser = _mapper.Map<User>(existingUserResponse);

            if (request.Username == null &&
                request.Email == null &&
                request.Phone == null &&
                request.Location == null &&
                request.Image == null &&
                request.Status == null &&
                request.SkinTypeId == null)
            {
                return _mapper.Map<UserResponseModel>(existingUserResponse);
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                var emailUser = await _unitOfWork.Repository<User>()
                    .FindAsync(u => u.Email == request.Email && u.Id != id);

                if (emailUser != null)
                {
                    throw new InvalidOperationException("Email already exists.");
                }
            }

            // Cập nhật từng trường nếu có giá trị mới
            if (!string.IsNullOrEmpty(request.Username)) existingUserResponse.Username = request.Username;
            if (!string.IsNullOrEmpty(request.Email)) existingUserResponse.Email = request.Email;
            if (!string.IsNullOrEmpty(request.Phone)) existingUserResponse.Phone = request.Phone;
            if (!string.IsNullOrEmpty(request.Location)) existingUserResponse.Location = request.Location;
            if (!string.IsNullOrEmpty(request.Image)) existingUserResponse.Image = request.Image;
            if (request.Status.HasValue) existingUserResponse.Status = request.Status.Value;
            if (request.SkinTypeId.HasValue) existingUserResponse.SkinTypeId = request.SkinTypeId.Value;

            await _unitOfWork.Repository<User>().Update(existingUserResponse, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserResponseModel>(existingUserResponse);
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
