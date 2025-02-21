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
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IngredientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IngredientResponseModel>> GetAllAsync()
        {
            var ingredients = await _unitOfWork.Repository<Ingredient>().GetAll()
                .Where(i => !i.IsDelete)
                .Include(i => i.ProductIngredients).ThenInclude(pi => pi.Product).ToListAsync();
            return _mapper.Map<IEnumerable<IngredientResponseModel>>(ingredients);
        }

        public async Task<IngredientResponseModel?> GetByIdAsync(int id)
        {
            var ingredient = await _unitOfWork.Repository<Ingredient>().AsQueryable()
                .Include(i => i.ProductIngredients).ThenInclude(pi => pi.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
            return ingredient == null ? null : _mapper.Map<IngredientResponseModel>(ingredient);
        }

        public async Task<IngredientResponseModel> CreateAsync(IngredientRequestModel request)
        {
            var ingredient = _mapper.Map<Ingredient>(request);
            await _unitOfWork.Repository<Ingredient>().InsertAsync(ingredient);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create ingredient");
            return _mapper.Map<IngredientResponseModel>(ingredient);
        }

        public async Task<IngredientResponseModel?> UpdateAsync(int id, IngredientRequestModel request)
        {
            var existingIngredient = await _unitOfWork.Repository<Ingredient>().GetById(id);
            if (existingIngredient == null) return null;
            _mapper.Map(request, existingIngredient);
            await _unitOfWork.Repository<Ingredient>().Update(existingIngredient, id);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<IngredientResponseModel>(existingIngredient);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ingredient = await _unitOfWork.Repository<Ingredient>().GetById(id);
            if (ingredient == null) return false;
            ingredient.IsDelete = true;
            await _unitOfWork.Repository<Ingredient>().Update(ingredient, id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
