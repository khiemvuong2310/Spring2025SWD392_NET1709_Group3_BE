using DianaShop.Data.Entities;

namespace DianaShop.Service.Interfaces
{
    public interface IUserRoleService
    {
        Task AcceptRoleRequestAsync(int userId, int roleId);
        Task AddRoleAsync(UserRole role);
        Task<IEnumerable<UserRole>> GetAllPendingRoleRequestsAsync();
        Task<IEnumerable<string>> GetRolesOfUserAsync(int userId);
        Task RemoveRoleAsync(int userId, int roleId);
        Task RequestRoleAsync(int userId, int roleId);
        Task UpdateRolesAsync(int userId, List<int> newRoleIds);
        bool UserHasRole(int userId, string roleName);
    }
}