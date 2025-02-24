using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _categoryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updateResponse = await _categoryService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.DeleteAsync(id);
            if (!response) return NotFound();
            return NoContent();
        }
    }
}
