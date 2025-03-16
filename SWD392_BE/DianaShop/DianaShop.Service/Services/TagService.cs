using AutoMapper;
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
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagResponseModel>> GetAllAsync()
        {
            var tags = await _unitOfWork.Repository<Tag>().GetAll().Where(t => !t.IsDelete)
                .Include(t => t.ProductTags).ThenInclude(pt => pt.Product)
                .Include(t => t.PostTags).ThenInclude(pt => pt.Post)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TagResponseModel>>(tags);
        }

        public async Task<TagResponseModel?> GetByIdAsync(int id)
        {
            var tag = await _unitOfWork.Repository<Tag>().AsQueryable()
                .Include(t => t.ProductTags).ThenInclude(pt => pt.Product)
                .Include(t => t.PostTags).ThenInclude(pt => pt.Post)
                .FirstOrDefaultAsync(t => t.Id == id);
            return tag == null ? null : _mapper.Map<TagResponseModel>(tag);
        }

        public async Task<TagResponseModel> CreateAsync(TagRequestModel request)
        {
            var tag = _mapper.Map<Tag>(request);
            await _unitOfWork.Repository<Tag>().InsertAsync(tag);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create tag");
            return _mapper.Map<TagResponseModel>(tag);
        }

        public async Task<TagResponseModel?> UpdateAsync(int id, TagRequestModel request)
        {
            var existingTag = await _unitOfWork.Repository<Tag>().GetById(id);
            if (existingTag == null) return null;
            _mapper.Map(request, existingTag);
            await _unitOfWork.Repository<Tag>().Update(existingTag, id);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TagResponseModel>(existingTag);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var tag = await _unitOfWork.Repository<Tag>().GetById(id);
            if (tag == null) return;
            tag.IsDelete = true;
            await _unitOfWork.Repository<Tag>().Update(tag, id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            await _unitOfWork.Repository<Tag>().HardDelete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddTagToProductAsync(int tagId, int productId)
        {
            var productTag = new ProductTag { ProductId = productId, TagId = tagId };
            await _unitOfWork.Repository<ProductTag>().InsertAsync(productTag);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveTagFromProductAsync(int tagId, int productId)
        {
            var productTag = await _unitOfWork.Repository<ProductTag>().AsQueryable()
                .FirstOrDefaultAsync(pt => pt.ProductId == productId && pt.TagId == tagId);
            if (productTag == null) return;
            await _unitOfWork.Repository<ProductTag>().DeleteAsync(productTag);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddTagToPostAsync(int tagId, int postId)
        {
            var postTag = new PostTag { PostId = postId, TagId = tagId };
            await _unitOfWork.Repository<PostTag>().InsertAsync(postTag);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveTagFromPostAsync(int tagId, int postId)
        {
            var postTag = await _unitOfWork.Repository<PostTag>().AsQueryable()
                .FirstOrDefaultAsync(pt => pt.PostId == postId && pt.TagId == tagId);
            if (postTag == null) return;
            await _unitOfWork.Repository<PostTag>().DeleteAsync(postTag);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
