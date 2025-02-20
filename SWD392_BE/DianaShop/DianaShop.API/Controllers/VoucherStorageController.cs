using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherStorageController : ControllerBase
    {
        private readonly IVoucherStorageService _service;

        public VoucherStorageController(IVoucherStorageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherStorageRequestModel request)
        {
            return Ok(await _service.CreateAsync(request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.DeleteAsync(id));
        }

        [HttpPost("add-voucher")]
        public async Task<IActionResult> AddVoucherToUser([FromBody] AddVoucherStorageRequestModel request)
        {
            var result = await _service.AddVoucherToUserAsync(request);
            return result ? Ok("Voucher added successfully.") : BadRequest("Failed to add voucher.");
        }
    }
}
