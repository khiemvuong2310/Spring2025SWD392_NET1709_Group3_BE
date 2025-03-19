using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _service.GetAllAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _service.GetByIdAsync(id);
            if (comment == null) return NotFound("Comment not found.");
            return Ok(comment);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CommentRequestModel request)
        {
            var comment = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] CommentUpdateRequestModel request)
        {
            var result = await _service.UpdateAsync(id, request);
            return result ? Ok("Comment updated successfully.") : NotFound("Comment not found.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Comment not found" });
            return Ok(new { message = "Comment deleted successfully" });
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var comments = await _service.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }
    }
}
