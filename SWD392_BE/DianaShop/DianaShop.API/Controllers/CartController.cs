using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        //private readonly ICartService _cartService;

        //public CartController(ICartService cartService)
        //{
        //    _cartService = cartService;
        //}

        //// Lấy tất cả giỏ hàng của người dùng
        //[HttpGet("user/{userId}")]
        //public async Task<ActionResult<IEnumerable<CartResponseModel>>> GetAllCarts(int userId)
        //{
        //    var carts = await _cartService.GetAllCartsAsync(userId);
        //    return Ok(carts);
        //}

        //// Lấy giỏ hàng theo ID
        //[HttpGet("{cartId}")]
        //public async Task<ActionResult<CartResponseModel>> GetCartById(int cartId)
        //{
        //    var cart = await _cartService.GetCartByIdAsync(cartId);
        //    if (cart == null) return NotFound("Cart not found");
        //    return Ok(cart);
        //}

        ////Tạo giỏ hàng mới
        //[HttpPost]
        //public async Task<ActionResult<CartResponseModel>> CreateCart([FromBody] CartRequestModel request)
        //{
        //    var createdCart = await _cartService.CreateCartAsync(request);
        //    return CreatedAtAction(nameof(GetCartById), new { cartId = createdCart.CartID }, createdCart);
        //}

        ////  Cập nhật giỏ hàng
        //[HttpPut("{cartId}")]
        //public async Task<ActionResult<CartResponseModel>> UpdateCart(int cartId, [FromBody] CartRequestModel request)
        //{
        //    var updatedCart = await _cartService.UpdateCartAsync(cartId, request);
        //    if (updatedCart == null) return NotFound("Cart not found");
        //    return Ok(updatedCart);
        //}

        ////  Xóa giỏ hàng
        //[HttpDelete("{cartId}")]
        //public async Task<ActionResult> DeleteCart(int cartId)
        //{
        //    var success = await _cartService.DeleteCartAsync(cartId);
        //    if (!success) return NotFound("Cart not found");
        //    return NoContent();
        //}

        ////  Thêm một sản phẩm vào giỏ hàng
        //[HttpPost("{cartId}/add-product/{productId}")]
        //public async Task<ActionResult<CartResponseModel>> AddProductToCart(int cartId, int productId)
        //{
        //    var updatedCart = await _cartService.AddProductToCartAsync(cartId, productId);
        //    if (updatedCart == null) return NotFound("Cart or product not found");
        //    return Ok(updatedCart);
        //}
    }
}
