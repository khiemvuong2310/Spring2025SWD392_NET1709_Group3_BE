using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface ISkincareRoutineService
    {
        Task<PaginatedResponse<SkincareRoutineResponseModel>> GetAllAsync(SkincareRoutineQueryRequest request);
        Task<SkincareRoutineResponseModel?> GetRoutineByIdAsync(int id);
        Task<RoutineStepResponseModel?> GetRoutineStepByIdAsync(int id);
        Task<SkincareRoutineResponseModel?> CreateRoutineAsync(SkincareRoutineRequestModel request);
        Task<SkincareRoutineResponseModel?> AddRoutineStepAsync(int routineId, RoutineStepRequestModel request);
        Task UpdateRoutineAsync(int id, SkincareRoutineRequestModel request);
        Task UpdateRoutineStepAsync(int id, RoutineStepRequestModel request);
        Task DeleteRoutineAsync(int id);
        Task<SkincareRoutineResponseModel?> DeleteRoutineStepAsync(int id);
        Task<PaginatedResponse<SkincareRoutineResponseModel>> GetSavedRoutine(int userId, SkincareRoutineQueryRequest request);
        Task SaveRoutine(int userId, int routineId);
        Task UnsaveRoutine(int userId, int routineId);
    }
}
