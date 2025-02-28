using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseModel> AddProductToCart(int userId, int productId, int quantity);
        Task<bool> ClearCart(int userId);
        Task<CartResponseModel> CreateCart(CartRequestModel request);
        Task<CartResponseModel> GetCartByUser(int userId);
        Task<CartResponseModel> RemoveProductFromCart(int userId, int productId);
        Task<CartResponseModel> UpdateCartProduct(int userId, int productId, int quantity);
    }
}