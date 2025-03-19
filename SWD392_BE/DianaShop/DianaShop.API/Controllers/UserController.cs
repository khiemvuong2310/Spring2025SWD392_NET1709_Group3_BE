using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Lấy tất cả người dùng
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Lấy người dùng theo ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token claims");
            }
            
            // Chuyển đổi ID từ claim sang int
            if (!int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return BadRequest("Invalid user ID in token");
            }

            bool isManagerOrStaff = User.IsInRole("Manager") || User.IsInRole("Staff");
            
            if (!isManagerOrStaff && currentUserId != id)
            {
                return Forbid("You can only view your own user information");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            
            return Ok(user);
        }

        // Tạo người dùng mới
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // Cập nhật người dùng
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUserAsync(id, request);
            if (updatedUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(updatedUser);
        }

        // Xóa người dùng
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound("User not found.");
            }

            return NoContent();  
        }

        [HttpPut("UpdatePassword/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordRequest request)
        {
            try
            {
                var result = await _userService.UpdatePasswordAsync(id, request.OldPassword, request.NewPassword);

                if (!result)
                {
                    return BadRequest(new { message = "Invalid old password or user not found." });
                }

                return Ok(new { message = "Password updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
