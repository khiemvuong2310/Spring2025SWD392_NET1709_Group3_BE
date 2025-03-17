using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }


        [HttpGet]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _storageService.GetAllStoragesAsync());
        }

        [HttpGet("{voucherId}/{storageId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int voucherId, int storageId)
        {
            var storage = await _storageService.GetStorageByIdAsync(voucherId, storageId);
            if (storage == null) return NotFound();
            return Ok(storage);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] StorageRequestModel request)
        {
            var storage = await _storageService.CreateStorageAsync(request);
            return CreatedAtAction(nameof(GetById), new { voucherId = storage.VoucherId, storageId = storage.VoucherStorageId }, storage);
        }


        [HttpPut("{voucherId}/{storageId}")]
        [Authorize]
        public async Task<IActionResult> Update(int voucherId, int storageId, [FromBody] StorageRequestModel request)
        {
            var updatedStorage = await _storageService.UpdateStorageAsync(voucherId, storageId, request);
            if (updatedStorage == null) return NotFound();
            return Ok(updatedStorage);
        }

        [HttpDelete("{voucherId}/{storageId}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> Delete(int voucherId, int storageId)
        {
            var success = await _storageService.DeleteStorageAsync(voucherId, storageId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("storages/voucher/{voucherId}")]
        public async Task<IActionResult> GetStoragesByVoucherId(int voucherId)
        {
            var storages = await _storageService.GetStoragesByVoucherIdAsync(voucherId);
            return Ok(storages);
        }
    }
}
