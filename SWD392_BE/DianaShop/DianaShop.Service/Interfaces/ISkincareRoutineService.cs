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
        Task DeleteRoutineAsync(int id);
        Task<SkincareRoutineResponseModel?> DeleteRoutineStepAsync(int id);
    }
}
