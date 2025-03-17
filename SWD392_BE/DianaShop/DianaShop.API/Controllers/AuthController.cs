using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authServices)
        {
            _authService = authServices;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var result = await _authService.AuthenticateAsync(model.Email, model.Password);

            return StatusCode((int)result.Code, result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return StatusCode((int)result.Code, result);
        }

        [HttpPost("send-email")]
        public async Task<ActionResult> Gets(int userId)
        {
            var result = await _authService.SendAccount(userId);
            return StatusCode((int)result.Code, result);
        }

        [HttpPut("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] DianaShop.Service.RequestModel.ForgotPasswordRequest request)
        {
            var result = await _authService.ForgotPassword(request);
            return StatusCode((int)result.Code, result);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            return StatusCode((int)result.Code, result);
        }

        [HttpPost("admin-create-account")]
        [Authorize(Roles = "Manager")]
        public IActionResult AdminGenAcc(AdminCreateAccountModel model)
        {
            // Implement user registration logic here

            // Once the user is registered, generate JWT token
            //return Ok(_authService.RegisterAsync(model).Result);
            var result = _authService.AdminGenAcc(model).Result;
            return StatusCode((int)result.Code, result);
        }
    }
}
