using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IStorageService
    {
        Task<StorageResponseModel> CreateStorageAsync(StorageRequestModel request);
        Task<bool> DeleteStorageAsync(int voucherId, int storageId);
        Task<List<StorageResponseModel>> GetAllStoragesAsync();
        Task<StorageResponseModel> GetStorageByIdAsync(int voucherId, int storageId);
        Task<StorageResponseModel> UpdateStorageAsync(int voucherId, int storageId, StorageRequestModel request);
        Task<List<StorageResponseModel>> GetStoragesByVoucherIdAsync(int voucherId);
    }
}