using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherStorageController : ControllerBase
    {
        private readonly IVoucherStorageService _voucherStorageService;

        public VoucherStorageController(IVoucherStorageService voucherStorageService)
        {
            _voucherStorageService = voucherStorageService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _voucherStorageService.GetAllVoucherStoragesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _voucherStorageService.GetVoucherStorageByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> Create([FromBody] VoucherStorageRequestModel request)
        {
            var result = await _voucherStorageService.CreateVoucherStorageAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _voucherStorageService.DeleteVoucherStorageAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        //[HttpPost("add-voucher")]
        //public async Task<IActionResult> AddVoucherToUser([FromBody] AddVoucherStorageRequestModel request)
        //{
        //    var result = await _service.AddVoucherToUserAsync(request);
        //    return result ? Ok("Voucher added successfully.") : BadRequest("Failed to add voucher.");
        //}
    }
}
