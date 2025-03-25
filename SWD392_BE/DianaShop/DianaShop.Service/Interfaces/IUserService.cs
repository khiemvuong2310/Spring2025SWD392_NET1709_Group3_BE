using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;

namespace DianaShop.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel> CreateUserAsync(UserCreateRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserResponseModel>> GetAllUsersAsync();
        Task<UserResponseModel?> GetUserByIdAsync(int id);
        Task<UserResponseModel?> UpdateUserAsync(int id, UserUpdateRequest request);
        Task<UserResponseModel?> AddProfileImageAsync(int id, IFormFile imageFile);
        Task<bool> UpdatePasswordAsync(int id, string oldPassword, string newPassword);
        Task<bool> UpdateWalletAsync(int id, decimal? wallet);
        Task<UserResponseModel?> GetUserByUsernameAsync(string username);
        Task<UserResponseModel?> GetUserByEmailAsync(string email);
        Task AddUserSkinTypeAsync(int userId, RoutineOrUserSkinTypeRequest request);
        Task UpdateUserSkinTypeAsync(int userId, RoutineOrUserSkinTypeRequest request);
        Task DeleteUserSkinTypeAsync(int userId, int skinTypeId);
        Task<IEnumerable<RoutineOrUserSkinTypeResponse>> GetUserSkinTypesAsync(int userId);
    }
}