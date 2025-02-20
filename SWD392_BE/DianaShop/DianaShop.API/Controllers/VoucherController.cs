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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _voucherService.GetAllVouchersAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var voucher = await _voucherService.GetVoucherByIdAsync(id);
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherRequestModel request)
        {
            var voucher = await _voucherService.CreateVoucherAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = voucher.Id }, voucher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VoucherRequestModel request)
        {
            var updatedVoucher = await _voucherService.UpdateVoucherAsync(id, request);
            if (updatedVoucher == null) return NotFound();
            return Ok(updatedVoucher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _voucherService.DeleteVoucherAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
