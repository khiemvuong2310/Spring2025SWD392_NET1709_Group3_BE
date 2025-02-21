using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class FormulationTypeController : BaseController
    {
        private readonly IFormulationTypeService _formulationTypeService;
        public FormulationTypeController(IFormulationTypeService formulationTypeService)
        {
            _formulationTypeService = formulationTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _formulationTypeService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _formulationTypeService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FormulationTypeRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _formulationTypeService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FormulationTypeRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updateResponse = await _formulationTypeService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _formulationTypeService.DeleteAsync(id);
            if (!response) return NotFound();
            return NoContent();
        }
    }
}
