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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Repository<User>()
                .Include(u => u.SkinType)
                .Include(u => u.Cart)
                .Include(u => u.VoucherStorage)
                .Include(u => u.UserRoles)
                .Include(u => u.FeedBacks)
                .Include(u => u.Orders)
                .Where(u => !u.IsDelete)
                .ToListAsync();
        }

        // Lấy người dùng theo ID
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.Repository<User>()
                .Include(u => u.SkinType)
                .Include(u => u.Cart)
                .Include(u => u.VoucherStorage)
                .Include(u => u.UserRoles)
                .Include(u => u.FeedBacks)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);
        }

        // Tạo mới người dùng
        public async Task<User> CreateUserAsync(UserCreateRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.Password = PasswordTools.HashPassword(user.Password);

            await _unitOfWork.Repository<User>().InsertAsync(user);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0) throw new Exception("Failed to create user");

            return user;
        }

        // Cập nhật thông tin người dùng
        public async Task<UserResponseModel?> UpdateUserAsync(int id, UserCreateRequest request)
        {
            var existingUser = await _unitOfWork.Repository<User>().GetById(id);

            if (existingUser == null) return null;

            _mapper.Map(request, existingUser);

            // Nếu thay đổi mật khẩu, mã hóa lại
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
            if (user == null || !PasswordTools.VerifyPassword(oldPassword, user.Password))
            {
                return false;
            }

            user.Password = PasswordTools.HashPassword(newPassword);
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
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _unitOfWork.Repository<User>()
                .GetAll()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDelete);
        }

        // Lấy người dùng theo email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.Repository<User>()
                .GetAll()
                .FirstOrDefaultAsync(u => EF.Functions.Collate(u.Email, "utf8mb4_bin") == email && !u.IsDelete);
        }

    }


}
