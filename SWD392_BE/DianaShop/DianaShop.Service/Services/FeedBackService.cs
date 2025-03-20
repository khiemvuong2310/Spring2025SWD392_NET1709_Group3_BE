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
    public class FeedBackService : IFeedBackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedBackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Lấy tất cả FeedBack
        public async Task<IEnumerable<FeedBackResponseModel>> GetAllFeedBacksAsync()
        {
            var feedbacks = await _unitOfWork.Repository<FeedBack>()
                .Include(f => f.Product)
                .Include(f => f.User)
                .Where(f => !f.IsDelete) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<FeedBackResponseModel>>(feedbacks);
        }

        // Lấy FeedBack theo ID
        public async Task<FeedBackResponseModel?> GetFeedBackByIdAsync(int id)
        {
            var feedback = await _unitOfWork.Repository<FeedBack>()
                .Include(f => f.Product)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDelete);

            return feedback == null ? null : _mapper.Map<FeedBackResponseModel>(feedback);
        }

        // Tạo mới FeedBack
        public async Task<FeedBackResponseModel> CreateFeedBackAsync(FeedBackRequestModel request)
        {
            var feedback = _mapper.Map<FeedBack>(request); 
            await _unitOfWork.Repository<FeedBack>().InsertAsync(feedback);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0) throw new Exception("Failed to create feedback");

            return _mapper.Map<FeedBackResponseModel>(feedback); // Ánh xạ từ FeedBack entity sang FeedBackResponseModel
        }

        // Cập nhật FeedBack
        public async Task<FeedBackResponseModel?> UpdateFeedBackAsync(int id, FeedBackRequestModel request)
        {
            var existingFeedBack = await _unitOfWork.Repository<FeedBack>().GetById(id);

            if (existingFeedBack == null) return null;

            _mapper.Map(request, existingFeedBack); // Ánh xạ từ FeedBackRequestModel vào FeedBack entity
            await _unitOfWork.Repository<FeedBack>().Update(existingFeedBack, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FeedBackResponseModel>(existingFeedBack); // Ánh xạ từ FeedBack entity sang FeedBackResponseModel
        }


        // Xóa FeedBack
        public async Task<bool> DeleteFeedBackAsync(int id)
        {
            var feedback = await _unitOfWork.Repository<FeedBack>().GetById(id);
            if (feedback == null) return false;

            feedback.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
