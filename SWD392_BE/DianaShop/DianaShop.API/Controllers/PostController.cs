using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace DianaShop.API.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: api/post
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PostQueryRequest request)
        {
            var response = await _postService.GetAllAsync(request);
            return Ok(response);
        }

        // GET: api/post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _postService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [Authorize(Roles = "Staff,Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PostRequestModel request)
        {           
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null) return Unauthorized();

            var response = await _postService.AddAsync(request, int.Parse(userIdString));
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        //[Authorize(Roles = "Staff,Manager")]
        [HttpPut("publication/{id}")]
        public async Task<IActionResult> UpdatePublicationStatus(int id)
        {
            var response = await _postService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _postService.UpdatePublicationStatusAsync(id);
            return NoContent();
        }

        //[Authorize(Roles = "Staff,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] PostRequestModel request)
        {
            var response = await _postService.GetByIdAsync(id);
            if (response == null) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _postService.UpdateAsync(id, request);
            return NoContent();
        }

        [Authorize(Roles = "Staff,Manager")]
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var response = await _postService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _postService.SoftDeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("hard-delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _postService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _postService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
