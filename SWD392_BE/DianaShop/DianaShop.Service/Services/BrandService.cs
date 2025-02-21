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
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandResponseModel>> GetAllAsync()
        {
            var brands = await _unitOfWork.Repository<Brand>().GetAll()
                .Where(b => !b.IsDelete)
                .Include(b => b.Products).ThenInclude(pr => pr.Packaging)
                .Include(b => b.Products).ThenInclude(pr => pr.Category)
                .Include(b => b.Products).ThenInclude(pr => pr.FormulationType)
                .ToListAsync();
            brands = brands.Where(p => !p.IsDelete).ToList();
            return _mapper.Map<IEnumerable<BrandResponseModel>>(brands);
        }

        public async Task<BrandResponseModel?> GetByIdAsync(int id)
        {
            var brand = await _unitOfWork.Repository<Brand>().AsQueryable()
                .Include(b => b.Products).ThenInclude(pr => pr.Packaging)
                .Include(b => b.Products).ThenInclude(pr => pr.Category)
                .Include(b => b.Products).ThenInclude(pr => pr.FormulationType)
                .FirstOrDefaultAsync(p => p.Id == id);
            return brand == null ? null : _mapper.Map<BrandResponseModel>(brand);
        }

        public async Task<BrandResponseModel> CreateAsync(BrandRequestModel request)
        {
            var brand = _mapper.Map<Brand>(request);
            await _unitOfWork.Repository<Brand>().InsertAsync(brand);          
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create brand");
            return _mapper.Map<BrandResponseModel>(brand);
        }

        public async Task<BrandResponseModel?> UpdateAsync(int id, BrandRequestModel request)
        {
            var existingBrand = await _unitOfWork.Repository<Brand>().GetById(id);
            if (existingBrand == null) return null;

            _mapper.Map(request, existingBrand);
            await _unitOfWork.Repository<Brand>().Update(existingBrand, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BrandResponseModel>(existingBrand);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _unitOfWork.Repository<Brand>().GetById(id);
            if (brand == null) return false;
            brand.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
