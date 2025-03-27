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
            if (request.RoutineSkinTypes.Any())
            {
                routine.RoutineSkinTypes = new List<RoutineSkinType>();
                foreach (var routineSkinType in request.RoutineSkinTypes)
                {
                    var skinType = await _unitOfWork.Repository<SkinType>().GetById(routineSkinType.SkinTypeId);
                    if (skinType != null)
                    {
                        routine.RoutineSkinTypes.Add(new RoutineSkinType
                        {
                            SkinTypeId = routineSkinType.SkinTypeId,
                            Percentage = routineSkinType.Percentage != 0 ? routineSkinType.Percentage / 100 : 0
                        });
                    }
                }
            }
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
            if (routineStep.Order > routine.RoutineSteps.Count + 1)
            {
                routineStep.Order = routine.RoutineSteps.Count + 1;
            }
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
        public async Task UpdateRoutineAsync(int id, SkincareRoutineRequestModel request)
        {
            var routine = await _unitOfWork.Repository<SkincareRoutine>().AsQueryable()
                .Where(r => r.Id == id)
                .Include (r => r.RoutineSkinTypes)
                .Include(r => r.RoutineSteps)
                .FirstOrDefaultAsync();
            if (routine == null) throw new Exception("Routine not found");
            _mapper.Map(request, routine);
            if (request.RoutineSkinTypes.Any())
            {
                routine.RoutineSkinTypes = new List<RoutineSkinType>();
                foreach (var routineSkinType in request.RoutineSkinTypes)
                {
                    var skinType = await _unitOfWork.Repository<SkinType>().GetById(routineSkinType.SkinTypeId);
                    if (skinType != null)
                    {
                        routine.RoutineSkinTypes.Add(new RoutineSkinType { SkinType = skinType, 
                            Percentage = routineSkinType.Percentage != 0 ? routineSkinType.Percentage / 100 : 0 });
                    }
                }
            }
            if (routine.RoutineSteps != null)
            {

                var routineSteps = await _unitOfWork.Repository<RoutineStep>()
                .AsQueryable()
                .Where(s => s.SkincareRoutineId == id)
                .ToListAsync();
                foreach (var step in routine.RoutineSteps)
                {
                    routineSteps.Add(step);
                }
                routineSteps = ReorderRoutineSteps(routineSteps);
                routine.RoutineSteps.Clear();
                routine.RoutineSteps = routineSteps;
            }
            
            await _unitOfWork.Repository<SkincareRoutine>().Update(routine, id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateRoutineStepAsync(int id, RoutineStepRequestModel request)
        {
            var routineStep = await _unitOfWork.Repository<RoutineStep>().GetById(id);
            if (routineStep == null) throw new Exception("Routine step not found");
            if (request.Order != routineStep.Order)
            {
                var routinesteps = _unitOfWork.Repository<RoutineStep>()
                    .AsQueryable()
                    .Where(s => s.SkincareRoutineId == routineStep.SkincareRoutineId)
                    .ToList();
                if (request.Order > routinesteps.Count + 1)
                {
                    request.Order = routinesteps.Count + 1;
                }
                else
                {
                    foreach (var step in routinesteps)
                    {
                        if (step.Order >= request.Order)
                        {
                            step.Order++;
                        }
                    }
                }              
            }
            _mapper.Map(request, routineStep);

            await _unitOfWork.Repository<RoutineStep>().Update(routineStep, id);
            await _unitOfWork.SaveChangesAsync();
        }
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

        public async Task<PaginatedResponse<SkincareRoutineResponseModel>> GetSavedRoutine(int userId, SkincareRoutineQueryRequest request)
        {
            var routines = _unitOfWork.Repository<SkincareRoutine>().GetAll()
                .Where(r => r.SavedRoutines.Any(ur => ur.UserId == userId))
                .Where(r => request.IsDeleted == null || r.IsDelete == request.IsDeleted)
                .Where(r => string.IsNullOrEmpty(request.Name) || r.Name.Contains(request.Name))
                .Where(r => !request.SkinTypeIds.Any() || r.RoutineSkinTypes.Any(rs => request.SkinTypeIds.Contains(rs.SkinTypeId)))
                .ProjectTo<SkincareRoutineResponseModel>(_mapper.ConfigurationProvider);
            return await PaginatedResponse<SkincareRoutineResponseModel>.CreateAsync(routines, request.PageNumber, request.PageSize);
        }

        public async Task SaveRoutine(int userId, int routineId)
        {
            var savedRoutine = await _unitOfWork.Repository<SavedRoutine>()
                .AsQueryable()
                .Where(ur => ur.UserId == userId && ur.RoutineId == routineId)
                .FirstOrDefaultAsync();
            if (savedRoutine != null) return;
            savedRoutine = new SavedRoutine
            {
                UserId = userId,
                RoutineId = routineId
            };
            await _unitOfWork.Repository<SavedRoutine>().InsertAsync(savedRoutine);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UnsaveRoutine(int userId, int routineId)
        {
            var savedRoutine = await _unitOfWork.Repository<SavedRoutine>()
                .AsQueryable()
                .Where(ur => ur.UserId == userId && ur.RoutineId == routineId)
                .FirstOrDefaultAsync();
            if (savedRoutine == null) return;
            _unitOfWork.Repository<SavedRoutine>().Remove(savedRoutine);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
