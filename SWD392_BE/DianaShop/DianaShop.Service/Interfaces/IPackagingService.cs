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
        Task<IEnumerable<PackagingResponseModel>> GetAllPackagingsAsync();
        Task<PackagingResponseModel?> GetPackagingByIdAsync(int id);
        Task<PackagingResponseModel> CreatePackagingAsync(PackagingRequestModel request);
        Task<PackagingResponseModel?> UpdatePackagingAsync(int id, PackagingRequestModel request);
        Task<bool> DeletePackagingAsync(int id);

    }
}
