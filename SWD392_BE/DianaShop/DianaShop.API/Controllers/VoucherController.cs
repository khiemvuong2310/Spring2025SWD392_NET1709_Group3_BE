using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromBody] VoucherRequestModel request)
        {
            var result = await _voucherService.CreateVoucherAsync(request);
            return CreatedAtAction(nameof(GetVoucherById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherById(int id)
        {
            var result = await _voucherService.GetVoucherByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVouchers()
        {
            var results = await _voucherService.GetAllVouchersAsync();
            return Ok(results);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(int id, [FromBody] VoucherRequestModel request)
        {
            var result = await _voucherService.UpdateVoucherAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var success = await _voucherService.DeleteVoucherAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
