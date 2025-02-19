using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseModel> CreateCartAsync(CartRequestModel request);
        Task<IEnumerable<CartResponseModel>> GetAllCartsAsync(int userId);
        Task<CartResponseModel?> GetCartByIdAsync(int cartId);
        Task<bool> RemoveProductFromCartAsync(int cartId);
        Task<CartResponseModel?> UpdateCartAsync(int cartId, CartRequestModel request);
    }
}