using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromForm]SkincareRoutineRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.CreateRoutineAsync(model);
            return Ok(response);
        }
        [HttpPost("{routineId}/step")]
        public async Task<IActionResult> AddStep(int routineId, [FromForm]RoutineStepRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.AddRoutineStepAsync(routineId, model);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] SkincareRoutineRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.GetRoutineByIdAsync(id);
            if (response == null) return NotFound();
            await _skincareRoutineService.UpdateRoutineAsync(id, model);
            return NoContent();
        }

        [HttpPut("steps/{id}")]
        public async Task<IActionResult> UpdateStep(int id, [FromForm]RoutineStepRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skincareRoutineService.GetRoutineStepByIdAsync(id);
            if (response == null) return NotFound();
            await _skincareRoutineService.UpdateRoutineStepAsync(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _skincareRoutineService.GetRoutineByIdAsync(id);
            if (response == null) return NotFound();
            await _skincareRoutineService.DeleteRoutineAsync(id);
            return NoContent();
        }

        [HttpDelete("steps/{id}")]
        public async Task<IActionResult> DeleteStep(int id)
        {
            var response = await _skincareRoutineService.DeleteRoutineStepAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
