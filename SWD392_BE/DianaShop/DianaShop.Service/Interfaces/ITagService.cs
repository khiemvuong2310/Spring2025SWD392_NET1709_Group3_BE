using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagResponseModel>> GetAllAsync();
        Task<TagResponseModel?> GetByIdAsync(int id);
        Task<TagResponseModel> CreateAsync(TagRequestModel request);
        Task<TagResponseModel?> UpdateAsync(int id, TagRequestModel request);
        Task SoftDeleteAsync(int id);
        Task HardDeleteAsync(int id);
        Task AddTagToProductAsync(int tagId, int productId);
        Task RemoveTagFromProductAsync(int tagId, int productId);
        Task AddTagToPostAsync(int tagId, int postId);
        Task RemoveTagFromPostAsync(int tagId, int postId);
    }
}
