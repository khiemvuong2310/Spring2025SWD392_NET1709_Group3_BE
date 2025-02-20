using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IFeedBackService
    {
        Task<FeedBackResponseModel> CreateFeedBackAsync(FeedBackRequestModel request);
        Task<bool> DeleteFeedBackAsync(int id);
        Task<IEnumerable<FeedBackResponseModel>> GetAllFeedBacksAsync();
        Task<FeedBackResponseModel?> GetFeedBackByIdAsync(int id);
        Task<FeedBackResponseModel?> UpdateFeedBackAsync(int id, FeedBackRequestModel request);
    }
}