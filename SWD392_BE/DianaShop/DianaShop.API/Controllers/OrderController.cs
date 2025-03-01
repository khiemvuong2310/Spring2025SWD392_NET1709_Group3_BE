using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        /// 🛒 Lấy tất cả order của User
        [HttpGet("user/{userId}")]  // Đổi {userId} thành "user/{userId}"
        public async Task<IActionResult> GetAllOrders(int userId)
        {
            var response = await _orderService.GetAllOrderByUser(userId);
            return Ok(response);
        }

        /// 🛒 Lấy order theo ID
        [HttpGet("id/{orderId}")]  // Đổi {orderId} thành "id/{orderId}"
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var response = await _orderService.GetOrderById(orderId);
            return Ok(response);
        }

        /// 🛒 Lấy tất cả order details của order
        [HttpGet("details/{orderId}")]  // Đổi {orderId} thành "details/{orderId}"
        public async Task<IActionResult> GetAllOrderDetails(int orderId)
        {
            var response = await _orderService.GetOrderDetails(orderId);
            return Ok(response);
        }

        /// 🛒 API tạo đơn hàng mới cho User
        [HttpPost("create order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestModel request)
        {
            if (request == null || request.UserID <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var order = await _orderService.CreateOrder(request);
            return Ok(order);
        }

        /// ➕ Thêm sản phẩm vào hóa đơn
        [HttpPost("add")]
        public async Task<IActionResult> AddOrderDetail([FromBody] OrderDetailRequestModel request)
        {
            try
            {
                var result = await _orderService.AddOrderDetail(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm trong giỏ hàng
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var result = await _orderService.GetOrderDetails(orderId);
            return Ok(result);
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong đơn hàng
        /// </summary>
        [HttpPut("{orderId}/{productId}")]
        public async Task<IActionResult> UpdateCartProduct([FromBody] OrderDetailRequestModel request)
        {
            try
            {
                var result = await _orderService.UpdateOrderDetail(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái trong đơn hàng
        /// </summary>
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateCartProduct(int orderId, [FromBody] string status)
        {
            try
            {
                var result = await _orderService.UpdateOrder(orderId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Xóa sản phẩm khỏi đơn hàng
        /// </summary>
        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(int orderId, int productId)
        {
            var result = await _orderService.RemoveOrderDetail(orderId, productId);
            if (!result) return NotFound(new { message = "Remove Fail" });

            return Ok(new { Message = "Order detail delete successfully!" });
        }

        /// 🗑️ Xóa tất cả sản phẩm trong hóa đơn
        [HttpDelete("clear/{orderId}")]
        public async Task<IActionResult> ClearOrder(int orderId)
        {
            var result = await _orderService.ClearOrder(orderId);
            if (!result)
                return BadRequest("Order is already empty or does not exist.");
            return Ok(new { Message = "Order cleared successfully!" });
        }

        /// 🗑️ Xóa tất cả sản phẩm trong hóa đơn
        [HttpDelete("Remove/{orderId}")]
        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            var result = await _orderService.ClearOrder(orderId);
            if (!result)
                return BadRequest("Order is already empty or does not exist.");
            return Ok(new { Message = "Order cleared successfully!" });
        }
    }
}
