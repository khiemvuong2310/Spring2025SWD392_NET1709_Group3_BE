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
    public class SkincareRoutineService : ISkincareRoutineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SkincareRoutineService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<SkincareRoutineResponseModel>> GetAllAsync(SkincareRoutineQueryRequest request)
        {
            var routines = _unitOfWork.Repository<SkincareRoutine>().GetAll()
                .Where(r => request.IsDeleted == null || r.IsDelete == request.IsDeleted)
                .Where(r => string.IsNullOrEmpty(request.Name) || r.Name.Contains(request.Name))
                .Where(r => !request.SkinTypeIds.Any() || r.RoutineSkinTypes.Any(rs => request.SkinTypeIds.Contains(rs.SkinTypeId)))
                .ProjectTo<SkincareRoutineResponseModel>(_mapper.ConfigurationProvider);
            return await PaginatedResponse<SkincareRoutineResponseModel>.CreateAsync(routines, request.PageNumber, request.PageSize);
        }

        public async Task<SkincareRoutineResponseModel?> GetRoutineByIdAsync(int id)
        {
            return await _unitOfWork.Repository<SkincareRoutine>()
                .AsQueryable()
                .Where(r => r.Id == id)
                .ProjectTo<SkincareRoutineResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<RoutineStepResponseModel?> GetRoutineStepByIdAsync(int id)
        {
            return await _unitOfWork.Repository<RoutineStep>()
                .AsQueryable()
                .Where(r => r.Id == id)
                .ProjectTo<RoutineStepResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<SkincareRoutineResponseModel?> CreateRoutineAsync(SkincareRoutineRequestModel request)
        {
            var routine = _mapper.Map<SkincareRoutine>(request);
            if (routine.RoutineSteps != null)
            {
                routine.RoutineSteps = ReorderRoutineSteps(routine.RoutineSteps);
            }
            await _unitOfWork.Repository<SkincareRoutine>().InsertAsync(routine);
            await _unitOfWork.SaveChangesAsync();
            return await GetRoutineByIdAsync(routine.Id);
        }

        public async Task<SkincareRoutineResponseModel?> AddRoutineStepAsync(int routineId, RoutineStepRequestModel request)
        {
            var routine = await _unitOfWork.Repository<SkincareRoutine>()
                .AsQueryable()
                .Where(r => r.Id == routineId)
                .Include(r => r.RoutineSteps)
                .FirstOrDefaultAsync();
            if (routine == null) return null;
            var routineStep = _mapper.Map<RoutineStep>(request);
            foreach (var step in routine.RoutineSteps)
            {
                if (step.Order >= request.Order)
                {
                    step.Order++;
                }
            }
            routine.RoutineSteps.Add(routineStep);
            routine.RoutineSteps = ReorderRoutineSteps(routine.RoutineSteps);
            await _unitOfWork.SaveChangesAsync();
            return await GetRoutineByIdAsync(routineId);
        }
        //public async Task UpdateRoutineAsync(int id, SkincareRoutineRequestModel request)
        //{
        //    var routine = await _unitOfWork.Repository<SkincareRoutine>().GetById(id);
        //    if (routine == null) throw new Exception("Routine not found");
        //    _mapper.Map(request, routine);
        //    if (request.RoutineSteps != null)
        //    {
        //        routine.RoutineSteps = request.RoutineSteps.Select(step => _mapper.Map<RoutineStep>(step)).ToList();
        //    }
        //    await _unitOfWork.Repository<SkincareRoutine>().Update(routine, id);
        //    await _unitOfWork.SaveChangesAsync();
        //}
        public async Task DeleteRoutineAsync(int id)
        {
            var routine = await _unitOfWork.Repository<SkincareRoutine>().GetById(id);
            if (routine == null) throw new Exception("Routine not found");
            routine.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<SkincareRoutineResponseModel?> DeleteRoutineStepAsync(int id)
        {
            var routineStep = await _unitOfWork.Repository<RoutineStep>().GetById(id);
            if (routineStep == null) return null;
            var routine = await _unitOfWork.Repository<SkincareRoutine>()
                .AsQueryable()
                .Where(r => r.Id == routineStep.SkincareRoutineId)
                .Include(r => r.RoutineSteps)
                .FirstOrDefaultAsync();
            if (routine == null) return null;

            routine.RoutineSteps.Remove(routineStep);
            _unitOfWork.Repository<RoutineStep>().Remove(routineStep);    
            
            routine.RoutineSteps = ReorderRoutineSteps(routine.RoutineSteps);

            await _unitOfWork.SaveChangesAsync();
            
            return await GetRoutineByIdAsync(routine.Id);
        }
        private List<RoutineStep> ReorderRoutineSteps(List<RoutineStep> routineSteps)
        {
            routineSteps = routineSteps.OrderBy(s => s.Order).ToList();
            for (int i = 0; i < routineSteps.Count; i++)
            {
                routineSteps[i].Order = i + 1;
            }
            return routineSteps;
        }
    }
}
