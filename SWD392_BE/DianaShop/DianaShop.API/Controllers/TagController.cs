using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _tagService.GetAllAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _tagService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _tagService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TagRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updateResponse = await _tagService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _tagService.DeleteAsync(id);
            if (!response) return NotFound();
            return NoContent();
        }

        [HttpPost("product/{productId}")]
        public async Task<IActionResult> AddTagToProduct(int tagId, int productId)
        {
            await _tagService.AddTagToProductAsync(tagId, productId);
            return NoContent();
        }

        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> RemoveTagFromProduct(int tagId, int productId)
        {
            await _tagService.RemoveTagFromProductAsync(tagId, productId);
            return NoContent();
        }

        [HttpPost("post/{postId}")]
        public async Task<IActionResult> AddTagToPost(int tagId, int postId)
        {
            await _tagService.AddTagToPostAsync(tagId, postId);            
            return NoContent();
        }

        [HttpDelete("post/{postId}")]
        public async Task<IActionResult> RemoveTagFromPost(int tagId, int postId)
        {
            await _tagService.RemoveTagFromPostAsync(tagId, postId);
            return NoContent();
        }
    }
}
