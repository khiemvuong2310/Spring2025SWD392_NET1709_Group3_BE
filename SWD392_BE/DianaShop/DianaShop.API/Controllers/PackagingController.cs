using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.Repositories;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
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
            var response = await _packagingService.GetAllAsync();
            return Ok(response);
        }

        // GET: api/packaging/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _packagingService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        // POST: api/packaging
        [HttpPost]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> Create([FromForm] PackagingRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _packagingService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT: api/packaging/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> Update(int id, [FromForm] PackagingRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _packagingService.GetByIdAsync(id);
            if (response == null) return NotFound();
            var updateResponse = await _packagingService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }

        // DELETE: api/packaging/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _packagingService.DeleteAsync(id);
            if (!response) return NotFound();
            return NoContent();
        }
    }
}
