using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DianaShop.Data.Entities;
using DianaShop.Repository.Repositories;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace DianaShop.Service.Services
{
    public class BatchService : IBatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BatchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BatchResponseModel>> GetAllAsync()
        {
            return await _unitOfWork.Repository<ProductBatch>().GetAll()
                .Where(b => !b.IsDelete)
                .ProjectTo<BatchResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<BatchResponseModel?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ProductBatch>().AsQueryable()
                .Where(b => b.Id == id)
                .ProjectTo<BatchResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<BatchResponseModel> CreateAsync(BatchRequestModel request)
        {
            var batch = _mapper.Map<ProductBatch>(request);
            if (await _unitOfWork.Repository<Product>().AnyAsync(x => x.Id == batch.ProductID) == false)
            {
                throw new ArgumentException("Product not found");
            }
            await _unitOfWork.Repository<ProductBatch>().InsertAsync(batch);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create batch");
            return _mapper.Map<BatchResponseModel>(batch);
        }

        public async Task<BatchResponseModel?> UpdateAsync(int id, BatchRequestModel request)
        {
            var existingBatch = await _unitOfWork.Repository<ProductBatch>().GetById(id);
            if (await _unitOfWork.Repository<Product>().AnyAsync(x => x.Id == request.ProductID) == false)
            {
                throw new ArgumentException("Product not found");
            }
            if (existingBatch == null) return null;
            _mapper.Map(request, existingBatch);
            await _unitOfWork.Repository<ProductBatch>().Update(existingBatch, id);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BatchResponseModel>(existingBatch);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var batch = await _unitOfWork.Repository<ProductBatch>().GetById(id);
            if (batch == null) return false;
            batch.IsDelete = true;
            await _unitOfWork.Repository<ProductBatch>().Update(batch, id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
