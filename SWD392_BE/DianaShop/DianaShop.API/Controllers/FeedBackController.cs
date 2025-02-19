using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;

        // Constructor nhận vào FeedBackService
        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        // Lấy tất cả FeedBacks
        [HttpGet]
        public async Task<IActionResult> GetAllFeedBacks()
        {
            var feedBacks = await _feedBackService.GetAllFeedBacksAsync();
            return Ok(feedBacks);
        }

        // Lấy FeedBack theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedBackById(int id)
        {
            var feedBack = await _feedBackService.GetFeedBackByIdAsync(id);
            if (feedBack == null)
            {
                return NotFound(new { message = "FeedBack not found" });
            }
            return Ok(feedBack);
        }

        // Tạo mới FeedBack
        [HttpPost]
        public async Task<IActionResult> CreateFeedBack([FromBody] FeedBackRequestModel request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid feedback data" });
            }

            try
            {
                var createdFeedBack = await _feedBackService.CreateFeedBackAsync(request);
                return CreatedAtAction(nameof(GetFeedBackById), new { id = createdFeedBack.Id }, createdFeedBack);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Cập nhật FeedBack
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedBack(int id, [FromBody] FeedBackRequestModel request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid feedback data" });
            }

            var updatedFeedBack = await _feedBackService.UpdateFeedBackAsync(id, request);
            if (updatedFeedBack == null)
            {
                return NotFound(new { message = "FeedBack not found" });
            }
            return Ok(updatedFeedBack);
        }

        // Xóa FeedBack
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedBack(int id)
        {
            var result = await _feedBackService.DeleteFeedBackAsync(id);
            if (!result)
            {
                return NotFound(new { message = "FeedBack not found" });
            }
            return NoContent();
        }
    }
}
