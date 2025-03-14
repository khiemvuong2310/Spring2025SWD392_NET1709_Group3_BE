using DianaShop.Data.Entities;
using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Mvc;
namespace DianaShop.API.Controllers
{
    public class SkinTypeController : BaseController
    {
        private readonly ISkinTypeService _skinTypeService;
        public SkinTypeController(ISkinTypeService skinTypeService)
        {
            _skinTypeService = skinTypeService;
        }
        // GET: api/skintype
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SkinTypeQueryRequest request)
        {
            var response = await _skinTypeService.GetAllAsync(request);
            return Ok(response);
        }
        // GET: api/skintype/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _skinTypeService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SkinTypeRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skinTypeService.AddAsync(model);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SkinTypeRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skinTypeService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _skinTypeService.UpdateAsync(id, model);
            return NoContent();
        }
        [HttpDelete("soft-deletion/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _skinTypeService.SoftDeleteAsync(id);
            var response = await _skinTypeService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return NoContent();
        }
        [HttpDelete("hard-deletion/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var response = await _skinTypeService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _skinTypeService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
