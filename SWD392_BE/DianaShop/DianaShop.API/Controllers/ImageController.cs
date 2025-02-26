using DianaShop.Data.Entities;
using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Mvc;
namespace DianaShop.API.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        // GET: api/image
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ImageQueryRequest request)
        {
            var response = await _imageService.GetAllAsync(request);
            return Ok(response);
        }
        // GET: api/image/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _imageService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(List<IFormFile> files)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _imageService.AddAsync(files);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IFormFile file)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _imageService.UpdateAsync(id, file);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _imageService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("product/{productId}")]
        public async Task<IActionResult> AddImageToProduct(int productId, int imageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _imageService.AddImageToProductAsync(productId, imageId);
            return NoContent();
        }

        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> RemoveImageFromProduct(int productId, int imageId)
        {
            await _imageService.RemoveImageFromProductAsync(productId, imageId);
            return NoContent();
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var response = await _imageService.GetImagesByProductIdAsync(productId);
            return Ok(response);
        }
        [HttpPost("post/{postId}")]
        public async Task<IActionResult> AddImageToPost(int postId, int imageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _imageService.AddImageToPostAsync(postId, imageId);
            return NoContent();
        }
        [HttpDelete("post/{postId}")]
        public async Task<IActionResult> RemoveImageFromPost(int postId, int imageId)
        {
            await _imageService.RemoveImageFromPostAsync(postId, imageId);
            return NoContent();
        }
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetImagesByPostId(int postId)
        {
            var response = await _imageService.GetImagesByPostIdAsync(postId);
            return Ok(response);
        }

    }
}
