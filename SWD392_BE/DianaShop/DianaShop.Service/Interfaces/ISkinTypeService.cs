using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface ISkinTypeService
    {
        Task<PaginatedResponse<SkinTypeResponseModel>> GetAllAsync(SkinTypeQueryRequest request);
        Task<SkinTypeResponseModel?> AddAsync(SkinTypeRequestModel model);
        Task<SkinTypeResponseModel?> GetByIdAsync(int id);
        Task UpdateAsync(int id, SkinTypeRequestModel model);
        Task SoftDeleteAsync(int id);
        Task HardDeleteAsync(int id);

    }
}
