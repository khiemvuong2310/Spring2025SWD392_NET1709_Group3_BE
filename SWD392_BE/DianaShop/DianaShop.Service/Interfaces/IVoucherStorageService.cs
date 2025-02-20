using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IVoucherStorageService
    {
        Task<List<VoucherStorageResponseModel>> GetAllVoucherStoragesAsync();
        Task<VoucherStorageResponseModel> GetVoucherStorageByIdAsync(int id);
        Task<VoucherStorageResponseModel> CreateVoucherStorageAsync(VoucherStorageRequestModel request);
        Task<bool> DeleteVoucherStorageAsync(int id);

    }
}