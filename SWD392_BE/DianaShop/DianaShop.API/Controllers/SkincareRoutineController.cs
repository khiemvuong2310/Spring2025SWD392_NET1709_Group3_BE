using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace DianaShop.API.Controllers
{
    public class SkincareRoutineController : BaseController
    {
        private readonly ISkincareRoutineService _skincareRoutineService;
        public SkincareRoutineController(ISkincareRoutineService skincareRoutineService)
        {
            _skincareRoutineService = skincareRoutineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SkincareRoutineQueryRequest request)
        {
            var response = await _skincareRoutineService.GetAllAsync(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoutineById(int id)
        {
            var response = await _skincareRoutineService.GetRoutineByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("steps/{id}")]
        public async Task<IActionResult> GetRoutineStepById(int id)
        {
            var response = await _skincareRoutineService.GetRoutineStepByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Create([FromForm]SkincareRoutineRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.CreateRoutineAsync(model);
            return Ok(response);
        }
        [HttpPost("{routineId}/step")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> AddStep(int routineId, [FromForm]RoutineStepRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.AddRoutineStepAsync(routineId, model);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Update(int id, [FromForm] SkincareRoutineRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.GetRoutineByIdAsync(id);
            if (response == null) return NotFound();
            await _skincareRoutineService.UpdateRoutineAsync(id, model);
            return NoContent();
        }

        [HttpPut("steps/{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> UpdateStep(int id, [FromForm]RoutineStepRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.GetRoutineStepByIdAsync(id);
            if (response == null) return NotFound();
            await _skincareRoutineService.UpdateRoutineStepAsync(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _skincareRoutineService.GetRoutineByIdAsync(id);
            if (response == null) return NotFound();
            await _skincareRoutineService.DeleteRoutineAsync(id);
            return NoContent();
        }

        [HttpDelete("steps/{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> DeleteStep(int id)
        {
            var response = await _skincareRoutineService.DeleteRoutineStepAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("saved")]
        [Authorize]
        public async Task<IActionResult> GetSavedRoutine([FromQuery] SkincareRoutineQueryRequest request)
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
            var response = await _skincareRoutineService.GetSavedRoutine(currentUserId, request);
            return Ok(response);
        }

        [HttpPost("{routineId}/save")]
        [Authorize]
        public async Task<IActionResult> SaveRoutine(int routineId)
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
            await _skincareRoutineService.SaveRoutine(currentUserId, routineId);
            return NoContent();
        }

        [HttpDelete("{routineId}/save")]
        [Authorize]
        public async Task<IActionResult> UnsaveRoutine(int routineId)
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
            await _skincareRoutineService.UnsaveRoutine(currentUserId, routineId);
            return NoContent();
        }
    }
}
