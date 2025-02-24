using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IBatchService
    {
        Task<IEnumerable<BatchResponseModel>> GetAllAsync();
        Task<BatchResponseModel?> GetByIdAsync(int id);
        Task<BatchResponseModel> CreateAsync(BatchRequestModel request);
        Task<BatchResponseModel?> UpdateAsync(int id, BatchRequestModel request);
        Task<bool> DeleteAsync(int id);
    }
}
