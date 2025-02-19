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
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CartResponseModel>>> GetAllCarts(int userId)
        {
            var carts = await _cartService.GetAllCartsAsync(userId);
            return Ok(carts);
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<CartResponseModel>> GetCartById(int cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<CartResponseModel>> CreateCart([FromBody] CartRequestModel request)
        {
            var createdCart = await _cartService.CreateCartAsync(request);
            return CreatedAtAction(nameof(GetCartById), new { cartId = createdCart.CartID }, createdCart);
        }

        [HttpPut("{cartId}")]
        public async Task<ActionResult<CartResponseModel>> UpdateCart(int cartId, [FromBody] CartRequestModel request)
        {
            var updatedCart = await _cartService.UpdateCartAsync(cartId, request);
            if (updatedCart == null) return NotFound();
            return Ok(updatedCart);
        }

        [HttpDelete("{cartId}")]
        public async Task<ActionResult> RemoveProductFromCart(int cartId)
        {
            var success = await _cartService.RemoveProductFromCartAsync(cartId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
