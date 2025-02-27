using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseModel?> AddProductsToCartAsync(int cartId, List<int> productIds);
        Task<CartResponseModel?> AddProductToCartAsync(int cartId, int productId);
        Task<CartResponseModel> CreateCartAsync(CartRequestModel request);
        Task<bool> DeleteCartAsync(int cartId);
        Task<IEnumerable<CartResponseModel>> GetAllCartsAsync(int userId);
        Task<CartResponseModel?> GetCartByIdAsync(int cartId);
        Task<CartResponseModel?> UpdateCartAsync(int cartId, CartRequestModel request);
    }
}