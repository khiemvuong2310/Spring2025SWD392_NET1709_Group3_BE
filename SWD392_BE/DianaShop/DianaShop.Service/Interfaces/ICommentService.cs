using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface ICommentService
    {
        Task<CommentResponseModel> CreateAsync(CommentRequestModel request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CommentResponseModel>> GetAllAsync();
        Task<CommentResponseModel> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, CommentUpdateRequestModel request);
    }
}