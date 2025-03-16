using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: api/brand
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _brandService.GetAllAsync();
            return Ok(response);
        }

        // GET: api/brand/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _brandService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BrandRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _brandService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] BrandRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _brandService.GetByIdAsync(id);
            if (response == null) return NotFound();
            var updateResponse = await _brandService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }

        [HttpDelete("soft-deletion/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _brandService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _brandService.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
