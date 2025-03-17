using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Mvc;
namespace DianaShop.API.Controllers
{
    public class IngredientController : BaseController
    {
        private readonly IIngredientService _ingredientService;
        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _ingredientService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _ingredientService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IngredientRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _ingredientService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] IngredientRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _ingredientService.GetByIdAsync(id);
            if (response == null) return NotFound();
            var updateResponse = await _ingredientService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _ingredientService.DeleteAsync(id);
            if (!response) return NotFound();
            return NoContent();
        }
    }
}
