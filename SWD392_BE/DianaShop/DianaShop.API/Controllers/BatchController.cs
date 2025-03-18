using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Authorize(Roles = "Staff")]
    public class BatchController : BaseController
    {
        private readonly IBatchService _batchService;
        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [Authorize(Roles = "Manager")]      
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _batchService.GetAllAsync();
            return Ok(response);
        }

        [Authorize(Roles = "Manager, Staff")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _batchService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [Authorize(Roles = "Manager, Staff")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BatchRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _batchService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [Authorize(Roles = "Manager, Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BatchRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _batchService.GetByIdAsync(id);
            if (response == null) return NotFound();
            var updateResponse = await _batchService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }

        [Authorize(Roles = "Manager, Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _batchService.DeleteAsync(id);
            if (!response) return NotFound();
            return NoContent();
        }
    }
}
