using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IImageService
    {
        Task<PaginatedResponse<ImageResponseModel>> GetAllAsync(ImageQueryRequest request);
        Task<ImageResponseModel?> GetByIdAsync(int id);
        Task<List<ImageResponseModel>> AddAsync(List<IFormFile> files);
        Task<ImageResponseModel?> UpdateAsync(int id, IFormFile file);
        Task DeleteAsync(int id);
        Task AddImageToProductAsync(int productId, int imageId);
        Task RemoveImageFromProductAsync(int productId, int imageId);
        Task<List<ImageResponseModel>> GetImagesByProductIdAsync(int productId);
        Task AddImageToPostAsync(int postId, int imageId);
        Task RemoveImageFromPostAsync(int postId, int imageId);
        Task<List<ImageResponseModel>> GetImagesByPostIdAsync(int postId);
        Task AddProfileImageAsync(int userId, IFormFile file);
    }
}
