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
    public class FormulationTypeService : IFormulationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FormulationTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FormulationTypeResponseModel>> GetAllAsync()
        {
            var types = await _unitOfWork.Repository<FormulationType>().GetAll()
                .Where(f => !f.IsDelete)
                .Include(f => f.Products).ThenInclude(pr => pr.Brand)
                .Include(f => f.Products).ThenInclude(pr => pr.Packaging)
                .Include(f => f.Products).ThenInclude(pr => pr.Category)
                .ToListAsync();
            types = types.Where(p => !p.IsDelete).ToList();
            return _mapper.Map<IEnumerable<FormulationTypeResponseModel>>(types);
        }

        public async Task<FormulationTypeResponseModel?> GetByIdAsync(int id)
        {
            var type = await _unitOfWork.Repository<FormulationType>().AsQueryable()
                .Include(f => f.Products).ThenInclude(pr => pr.Brand)
                .Include(f => f.Products).ThenInclude(pr => pr.Packaging)
                .Include(f => f.Products).ThenInclude(pr => pr.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            return type == null ? null : _mapper.Map<FormulationTypeResponseModel>(type);
        }

        public async Task<FormulationTypeResponseModel> CreateAsync(FormulationTypeRequestModel request)
        {
            var type = _mapper.Map<FormulationType>(request);
            await _unitOfWork.Repository<FormulationType>().InsertAsync(type);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create brand");
            return _mapper.Map<FormulationTypeResponseModel>(type);
        }

        public async Task<FormulationTypeResponseModel?> UpdateAsync(int id, FormulationTypeRequestModel request)
        {
            var existingType = await _unitOfWork.Repository<FormulationType>().GetById(id);
            if (existingType == null) return null;

            _mapper.Map(request, existingType);
            await _unitOfWork.Repository<FormulationType>().Update(existingType, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FormulationTypeResponseModel>(existingType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var type = await _unitOfWork.Repository<FormulationType>().GetById(id);
            if (type == null) return false;
            type.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
