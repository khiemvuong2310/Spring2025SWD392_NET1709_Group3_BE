using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostResponseModel>> GetAllAsync(PostQueryRequest request);
        Task<PostResponseModel?> AddAsync(PostRequestModel model, int userId);
        Task<PostResponseModel?> GetByIdAsync(int id);
        Task UpdatePublicationStatusAsync(int id);
        Task UpdateAsync(int id, PostRequestModel model);
        Task SoftDeleteAsync(int id);
        Task HardDeleteAsync(int id);

    }
}
