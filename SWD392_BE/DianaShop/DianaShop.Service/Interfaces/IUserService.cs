using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel> CreateUserAsync(UserCreateRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserResponseModel>> GetAllUsersAsync();
        Task<UserResponseModel?> GetUserByIdAsync(int id);
        Task<UserResponseModel?> UpdateUserAsync(int id, UserCreateRequest request);

        Task<bool> UpdatePasswordAsync(int id, string oldPassword, string newPassword);

        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
    }
}