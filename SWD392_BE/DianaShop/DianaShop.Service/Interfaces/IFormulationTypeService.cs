using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IFormulationTypeService
    {
        Task<FormulationTypeResponse> CreateAsync(FormulationTypeRequest request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<FormulationTypeResponse>> GetAllAsync();
        Task<FormulationTypeResponse> GetByIdAsync(int id);
        Task<FormulationTypeResponse> UpdateAsync(int id, FormulationTypeRequest request);
    }
}