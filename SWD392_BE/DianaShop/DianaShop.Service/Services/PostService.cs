using AutoMapper;
using AutoMapper.QueryableExtensions;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostResponseModel>> GetAllAsync(PostQueryRequest request)
        {
            var postsQuery = _unitOfWork.Repository<Post>()
                .GetAll()
                .Where(p => request.UserId == null || p.UserId == request.UserId)
                .Where(p => string.IsNullOrEmpty(request.Title) || p.Title.Contains(request.Title))
                .Where(p => p.Status == request.Status)
                .Where(p => request.MinCreatedDate == null || p.CreatedDate >= request.MinCreatedDate)
                .Where(p => request.MaxCreatedDate == null || p.CreatedDate <= request.MaxCreatedDate)
                .Where(p => request.MinPublishedDate == null || p.UpdatedDate >= request.MinPublishedDate)
                .Where(p => request.MaxPublishedDate == null || p.UpdatedDate <= request.MaxPublishedDate)
                .ProjectTo<PostResponseModel>(_mapper.ConfigurationProvider);
            postsQuery = request.SortBy switch
            {
                PostSort.Default => request.Ascending ? postsQuery.OrderBy(p => p.Id) : postsQuery.OrderByDescending(p => p.Id),
                PostSort.Title => request.Ascending ? postsQuery.OrderBy(p => p.Title) : postsQuery.OrderByDescending(p => p.Title),
                PostSort.CreatedDate => request.Ascending ? postsQuery.OrderBy(p => p.CreatedDate) : postsQuery.OrderByDescending(p => p.CreatedDate),
                PostSort.PublishedDate => request.Ascending ? postsQuery.OrderBy(p => p.PublishedDate) : postsQuery.OrderByDescending(p => p.PublishedDate),
                PostSort.UpdatedDate => request.Ascending ? postsQuery.OrderBy(p => p.UpdatedDate) : postsQuery.OrderByDescending(p => p.UpdatedDate),
                _ => postsQuery
            };

            return await postsQuery.Skip((request.PageNumber - 1) * request.PageSize)
                                   .Take(request.PageSize)
                                   .ToListAsync();
        }

        public async Task<PostResponseModel> AddAsync(PostRequestModel model, int userId)
        {
            var post = _mapper.Map<Post>(model);
            post.CreatedDate = DateTime.UtcNow;
            post.UserId = userId;
            if (model.TagIds != null)
            {
                foreach (var tagId in model.TagIds)
                {
                    post.PostTags.Add(new PostTag { TagId = tagId });
                }
            }
            if (model.ImageIds != null)
            {
                foreach (var ImageId in model.ImageIds)
                {
                    post.PostImages.Add(new PostImage { ImageId = ImageId });
                }
            }
            await _unitOfWork.Repository<Post>().InsertAsync(post);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PostResponseModel>(post);
        }

        public async Task<PostResponseModel?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Post>().AsQueryable()
                .Where(p => p.Id == id)
                .ProjectTo<PostResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePublicationStatusAsync(int id)
        {
            var post = await _unitOfWork.Repository<Post>().GetById(id);
            if (post == null) throw new Exception("Post not found");
            post.Status = !post.Status;
            post.PublishedDate = post.Status ? DateTime.UtcNow : null;
            await _unitOfWork.Repository<Post>().Update(post, id);
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateAsync(int id, PostRequestModel model)
        {
            var post = await _unitOfWork.Repository<Post>().GetById(id);
            if (post == null) throw new Exception("Post not found");

            _mapper.Map(model, post);
            post.UpdatedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Post>().Update(post, id);
            await _unitOfWork.CommitAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var post = await _unitOfWork.Repository<Post>().GetById(id);
            if (post == null) throw new Exception("Post not found");
            post.IsDelete = true;
            await _unitOfWork.Repository<Post>().Update(post, id);
            await _unitOfWork.CommitAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            var post = await _unitOfWork.Repository<Post>().GetById(id);
            if (post == null) throw new Exception("Post not found");
            _unitOfWork.Repository<Post>().Delete(post);
            await _unitOfWork.CommitAsync();
        }
    }

}
