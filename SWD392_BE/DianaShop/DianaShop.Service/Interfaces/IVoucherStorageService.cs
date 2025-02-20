using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IVoucherStorageService
    {
        Task<bool> AddVoucherToUserAsync(AddVoucherStorageRequestModel request);
        Task<VoucherStorageResponseModel> CreateAsync(VoucherStorageRequestModel request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<VoucherStorageResponseModel>> GetAllAsync();
        Task<VoucherStorageResponseModel> GetByIdAsync(int id);
    }
}