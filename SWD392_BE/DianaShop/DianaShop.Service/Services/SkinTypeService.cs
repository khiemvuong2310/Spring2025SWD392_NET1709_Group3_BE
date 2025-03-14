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
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.EntityFrameworkCore;

namespace DianaShop.Service.Services
{
    public class SkinTypeService : ISkinTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SkinTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<SkinTypeResponseModel>> GetAllAsync(SkinTypeQueryRequest request)
        {
            var skinTypes = _unitOfWork.Repository<SkinType>().GetAll()
                .Where(s => request.IsDeleted == null || s.IsDelete == request.IsDeleted)
                .Where(s => request.Name == null || s.Name.Contains(request.Name))
                .ProjectTo<SkinTypeResponseModel>(_mapper.ConfigurationProvider);
            return await PaginatedResponse<SkinTypeResponseModel>.CreateAsync(skinTypes, request.PageNumber, request.PageSize);
        }

        public async Task<SkinTypeResponseModel?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<SkinType>().GetAll()
                        .Where(s => s.Id == id)
                        .ProjectTo<SkinTypeResponseModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
        }

        public async Task<SkinTypeResponseModel?> AddAsync(SkinTypeRequestModel model)
        {
            var skinType = _mapper.Map<SkinType>(model);
            await _unitOfWork.Repository<SkinType>().InsertAsync(skinType);
            await _unitOfWork.CommitAsync();
            return await GetByIdAsync(skinType.Id);
        }

        public async Task UpdateAsync(int id, SkinTypeRequestModel model)
        {
            var skinType = await _unitOfWork.Repository<SkinType>().GetById(id);
            if (skinType == null) throw new Exception("Skin type not found");
            _mapper.Map(model, skinType);
            await _unitOfWork.Repository<SkinType>().Update(skinType, id);
            await _unitOfWork.CommitAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var skinType = await _unitOfWork.Repository<SkinType>().GetById(id);
            if (skinType == null) throw new Exception("Skin type not found");
            skinType.IsDelete = true;
            await _unitOfWork.Repository<SkinType>().Update(skinType, id);
            await _unitOfWork.CommitAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            await _unitOfWork.Repository<SkinType>().HardDelete(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
