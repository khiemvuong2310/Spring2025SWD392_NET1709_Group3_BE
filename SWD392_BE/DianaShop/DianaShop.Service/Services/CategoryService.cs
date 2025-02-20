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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponseModel>> GetAllAsync()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAll()
                .Where(c => !c.IsDelete)
                .Include(c => c.Products).ThenInclude(pr => pr.Packaging)
                .Include(c => c.Products).ThenInclude(pr => pr.Brand)
                .Include(c => c.Products).ThenInclude(pr => pr.FormulationType)
                .ToListAsync();
            categories = categories.Where(p => !p.IsDelete).ToList();
            return _mapper.Map<IEnumerable<CategoryResponseModel>>(categories);
        }

        public async Task<CategoryResponseModel?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().AsQueryable()
                .Include(c => c.Products).ThenInclude(pr => pr.Packaging)
                .Include(c => c.Products).ThenInclude(pr => pr.Brand)
                .Include(c => c.Products).ThenInclude(pr => pr.FormulationType)
                .FirstOrDefaultAsync(c => c.Id == id);
            return category == null ? null : _mapper.Map<CategoryResponseModel>(category);
        }

        public async Task<CategoryResponseModel> CreateAsync(CategoryRequestModel request)
        {
            var category = _mapper.Map<Category>(request);
            await _unitOfWork.Repository<Category>().InsertAsync(category);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create category");
            return _mapper.Map<CategoryResponseModel>(category);
        }

        public async Task<CategoryResponseModel?> UpdateAsync(int id, CategoryRequestModel request)
        {
            var existingCategory = await _unitOfWork.Repository<Category>().GetById(id);
            if (existingCategory == null) return null;

            _mapper.Map(request, existingCategory);
            await _unitOfWork.Repository<Category>().Update(existingCategory, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryResponseModel>(existingCategory);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetById(id);
            if (category == null) return false;
            category.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
