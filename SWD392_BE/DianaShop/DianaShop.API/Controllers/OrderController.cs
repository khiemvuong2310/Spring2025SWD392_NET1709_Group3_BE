using DianaShop.Data.Entities;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderService orderService, IOrderDetailService orderDetailService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _paymentService = paymentService;
        }

        /// 🛒 Lấy tất cả order
        [HttpGet("all")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllOrder();
            return Ok(response);
        }

        /// 🛒 Lấy tất cả order của User
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllOrdersByUser(int userId)
        {
            var response = await _orderService.GetAllOrderByUser(userId);
            return Ok(response);
        }

        /// 🛒 Lấy order theo ID
        [HttpGet("id/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var response = await _orderService.GetOrderById(orderId);
            return Ok(response);
        }

        /// 🛒 Lấy order theo thuộc tính
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SearchOrder([FromBody] OrderQueryRequest request)
        {
            var response = await _orderService.SearchOrder(request);
            return Ok(response);
        }

        /// 🛒 API tạo đơn hàng mới cho User
        [HttpPost("create order")]
        [Authorize]
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
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, int status)
        {
            try
            {
                var result = await _orderService.UpdateOrder(orderId, status);
                var response = await _orderService.GetOrderById(result.Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái trong đơn hàng
        /// </summary>
        [HttpPut("cancelorder/{orderId}")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var result = await _orderService.UpdateOrder(orderId, 6);
                var response = await _orderService.GetOrderById(result.Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        //thêm voucher
        [HttpPut("AddVoucher/{orderId}/{voucherId}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            var condition = await _orderDetailService.ClearOrderDetail(orderId);
            var result = await _orderService.RemoveOrder(orderId);
            if (!result)
                return BadRequest("Order does not exist.");
            return Ok(new { Message = "Order cleared successfully!" });
        }

        /// ✅ Thanh toán giỏ hàng
        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestModel request)
        {
            if (request == null || request.UserID <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var orderRequest = new OrderRequestModel
            {
                UserID = request.UserID,
                Address = request.Address,
                Shipprice = request.Shipprice
            };
            var success1 = await _orderService.CreateOrder(orderRequest);

            var listDetail = request.Details;
            //var listsuccess2 = List<OrderDetailResponseModel>();

            foreach (var item in listDetail)
            {
                try
                {
                    var detailRequest = new OrderDetailRequestModel
                    {
                        OrderID = success1.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };

                    var detail = await _orderDetailService.AddOrderDetail(detailRequest);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            if (request.voucherID != null && request.voucherID > 0)
            {
                try
                {
                    var voucherID = request.voucherID ?? 0;
                    var voucherResult = await _orderService.AddVoucherToOrder(success1.Id, voucherID);
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            var paymentRequest = new PaymentRequestModel
            {
                OrderID = success1.Id,
                PaymentMethodID = request.PaymentMethodID
            };
            var paymentResponse = await _paymentService.CreatePayment(paymentRequest);

            var orderResponse = await _orderService.GetOrderById(success1.Id);

            return Ok(new
            {
                Message = "Check out successfully",
                orderResponse,
                paymentResponse
            });
        }
    }
}
