using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class VNPayController : Controller
    {
        private readonly IUserService _usersService;
        private readonly IVnPayService _vpnPayService;

        private static int currentOrderId = 1;

        [HttpPost("vnpay")]
        public IActionResult PaymentCalls([FromBody] VnPaymentRequestModel requestModel)
        {
            // Tăng OrderId mỗi khi có người mới
            currentOrderId += 1;

            // Khởi tạo payload với các giá trị từ requestModel
            var payload = new VnPaymentRequestModel
            {
                //OrderId = currentOrderId, 
                FullName = requestModel.FullName,
                Description = requestModel.Description,
                Amount = requestModel.Amount,
                CreatedDate = DateTime.UtcNow.AddHours(7), // Đặt thời gian hiện tại (UTC+7)
                UserId = requestModel.UserId  // Include UserId in payload

            };


            // Tạo URL thanh toán
            var url = _vpnPayService.CreatePaymentUrl(HttpContext, payload);


            // Trả về link thanh toán
            return Ok(url);
        }



        [HttpGet("vnpay/api")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vpnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return StatusCode(500, new { message = $"Lỗi thanh toán VNPay: {response?.VnPayResponseCode ?? "unknown error"}" });
            }

            try
            {
                // Get user by ID
                var user = await _usersService.GetUserByIdAsync(response.UserId);
                if (user != null)
                {
                    // Update wallet balance
                    user.wallet = (user.wallet ?? 0) + response.Amount;
                    await _usersService.UpdateWalletAsync(user.Id, user.wallet);
                    await _vpnPayService.SaveTransactionAsync(response); // Save transaction to database
                                                                         //tạo 1 cái hàm lưu payload giống trên hàm post vào database,lưu giống kiểu của wallet dưới hàm get,tạo 1 api mới để get

                    return Ok(new
                    {
                        response,
                        newWalletBalance = user.wallet
                    });
                }

                return NotFound(new { message = $"User not found: {response.UserId}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error updating wallet: {ex.Message}" });
            }
        }

        [HttpGet("vnpay/transactions")]
        public async Task<IActionResult> GetTransactions(int? userId = null)
        {
            try
            {
                var transactions = await _vpnPayService.GetTransactionsAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
