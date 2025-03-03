using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IPackagingService
    {
        Task<IEnumerable<PackagingResponseModel>> GetAllAsync();
        Task<PackagingResponseModel?> GetByIdAsync(int id);
        Task<PackagingResponseModel> CreateAsync(PackagingRequestModel request);
        Task<PackagingResponseModel?> UpdateAsync(int id, PackagingRequestModel request);
        Task<bool> DeleteAsync(int id);

    }
}
