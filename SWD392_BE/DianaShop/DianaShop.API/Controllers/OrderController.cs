using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;

        public OrderController(IOrderService orderService, IOrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }



        /// 🛒 Lấy tất cả order
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllOrder();
            return Ok(response);
        }

        /// 🛒 Lấy tất cả order của User
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllOrdersByUser(int userId)
        {
            var response = await _orderService.GetAllOrderByUser(userId);
            return Ok(response);
        }

        /// 🛒 Lấy order theo ID
        [HttpGet("id/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var response = await _orderService.GetOrderById(orderId);
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

        /// <summary>
        /// Cập nhật trạng thái trong đơn hàng
        /// </summary>
        [HttpPut("status/{orderId}/{status}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, int status)
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

        //thêm voucher
        [HttpPut("status/{orderId}/{voucherId}")]
        public async Task<IActionResult> AddVoucher(int orderId, int voucherId)
        {
            try
            {
                var result = await _orderService.AddVoucherToOrder(orderId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// 🗑️ Xóa hóa đơn
        [HttpDelete("Remove/{orderId}")]
        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            var condition = await _orderDetailService.ClearOrderDetail(orderId);
            var result = await _orderService.RemoveOrder(orderId);
            if (!result)
                return BadRequest("Order does not exist.");
            return Ok(new { Message = "Order cleared successfully!" });
        }
    }
}
