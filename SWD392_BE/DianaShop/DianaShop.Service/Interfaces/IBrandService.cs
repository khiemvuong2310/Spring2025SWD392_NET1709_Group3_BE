using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseModel>> GetAllAsync();
        Task<BrandResponseModel?> GetByIdAsync(int id);
        Task<BrandResponseModel> CreateAsync(BrandRequestModel request);
        Task<BrandResponseModel?> UpdateAsync(int id, BrandRequestModel request);
        Task<bool> DeleteAsync(int id);
    }
}
