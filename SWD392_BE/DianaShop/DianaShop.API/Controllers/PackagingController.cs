using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.Repositories;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DianaShop.API.Controllers
{
    public class PackagingController : BaseController
    {
        private readonly IPackagingService _packagingService;

        public PackagingController(IPackagingService packagingService)
        {
            _packagingService = packagingService;
        }

        // GET: api/packaging
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _packagingService.GetAllPackagingsAsync();
            return Ok(response);
        }

        // GET: api/packaging/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _packagingService.GetPackagingByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        // POST: api/packaging
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PackagingRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _packagingService.CreatePackagingAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT: api/packaging/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PackagingRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var getResponse = await _packagingService.GetPackagingByIdAsync(id);
            if (getResponse == null) return NotFound();
            var updateResponse = await _packagingService.UpdatePackagingAsync(id, request);
            return Ok(updateResponse);
        }

        // DELETE: api/packaging/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _packagingService.DeletePackagingAsync(id);
            if (!response) return NotFound();
            return NoContent();
        }
    }
}
