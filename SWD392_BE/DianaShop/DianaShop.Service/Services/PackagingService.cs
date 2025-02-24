using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.Repositories;
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
    public class PackagingService : IPackagingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackagingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PackagingResponseModel>> GetAllAsync()
        {
            var packagings = await _unitOfWork.Repository<Packaging>().GetAll()
                .Where(p => !p.IsDelete)
                .Include(p => p.Products).ThenInclude(pr => pr.Brand)
                .Include(p => p.Products).ThenInclude(pr => pr.Category)
                .Include(p => p.Products).ThenInclude(pr => pr.FormulationType)
                .ToListAsync();
            packagings = packagings.Where(p => !p.IsDelete).ToList();
            return _mapper.Map<IEnumerable<PackagingResponseModel>>(packagings);
        }

        public async Task<PackagingResponseModel?> GetByIdAsync(int id)
        {
            var packaging = await _unitOfWork.Repository<Packaging>().AsQueryable()
                .Include(p => p.Products).ThenInclude(pr => pr.Brand)
                .Include(p => p.Products).ThenInclude(pr => pr.Category)
                .Include(p => p.Products).ThenInclude(pr => pr.FormulationType)
                .FirstOrDefaultAsync(p => p.Id == id);
            return packaging == null ? null : _mapper.Map<PackagingResponseModel>(packaging);
        }

        public async Task<PackagingResponseModel> CreateAsync(PackagingRequestModel request)
        {
            var packaging = _mapper.Map<Packaging>(request);
            await _unitOfWork.Repository<Packaging>().InsertAsync(packaging);          
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create packaging");
            return _mapper.Map<PackagingResponseModel>(packaging);
        }

        public async Task<PackagingResponseModel?> UpdateAsync(int id, PackagingRequestModel request)
        {
            var existingPackaging = await _unitOfWork.Repository<Packaging>().GetById(id);
            if (existingPackaging == null) return null;

            _mapper.Map(request, existingPackaging);
            await _unitOfWork.Repository<Packaging>().Update(existingPackaging, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PackagingResponseModel>(existingPackaging);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var packaging = await _unitOfWork.Repository<Packaging>().GetById(id);
            if (packaging == null) return false;
            packaging.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
