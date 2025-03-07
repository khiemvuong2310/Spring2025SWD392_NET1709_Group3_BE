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

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Lấy tất cả người dùng
        public async Task<IEnumerable<UserResponseModel>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Repository<User>()
                .Include(u => u.SkinType)
                .Include(u => u.VoucherStorage)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => !u.IsDelete)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserResponseModel>>(users);
        }

        // Lấy người dùng theo ID
        public async Task<UserResponseModel?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Repository<User>()
                .Include(u => u.SkinType)
                .Include(u => u.VoucherStorage)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);

            return _mapper.Map<UserResponseModel>(user);
        }

        // Tạo người dùng mới
        public async Task<UserResponseModel> CreateUserAsync(UserCreateRequest request)
        {
            if (!Regex.IsMatch(request.Phone, "^\\+?[0-9]{10,15}$"))
            {
                throw new ArgumentException("Invalid phone number format.");
            }

            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            // Ánh xạ UserCreateRequest -> User (theo cấu hình trong UserMapping)
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

            var createdUser = await _unitOfWork.Repository<User>()
                .Include(u => u.SkinType)
                .Include(u => u.VoucherStorage)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (createdUser == null)
            {
                throw new Exception("Failed to retrieve created user.");
            }

            // Trả về UserResponseModel (tránh vòng lặp)
            return _mapper.Map<UserResponseModel>(createdUser);
        }


        // Cập nhật thông tin người dùng
        public async Task<UserResponseModel?> UpdateUserAsync(int id, UserCreateRequest request)
        {
            if (!Regex.IsMatch(request.Phone, "^\\+?[0-9]{10,15}$"))
            {
                throw new ArgumentException("Invalid phone number format.");
            }
            var existingUser = await _unitOfWork.Repository<User>().GetById(id);

            if (existingUser == null) return null;

            var emailUser = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email && u.Id != id);
            if (emailUser != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            _mapper.Map(request, existingUser);

            if (!string.IsNullOrEmpty(request.Password))
            {
                existingUser.Password = PasswordTools.HashPassword(request.Password);
            }

            await _unitOfWork.Repository<User>().Update(existingUser, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserResponseModel>(existingUser);
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
            var user = await _unitOfWork.Repository<User>()
                .GetAll()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDelete);
            return _mapper.Map<UserResponseModel>(user);
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
