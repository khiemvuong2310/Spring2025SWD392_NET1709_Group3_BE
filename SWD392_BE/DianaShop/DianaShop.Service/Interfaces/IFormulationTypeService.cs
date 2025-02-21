using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IFormulationTypeService
    {
        Task<IEnumerable<FormulationTypeResponseModel>> GetAllAsync();
        Task<FormulationTypeResponseModel?> GetByIdAsync(int id);
        Task<FormulationTypeResponseModel> CreateAsync(FormulationTypeRequestModel request);
        Task<FormulationTypeResponseModel?> UpdateAsync(int id, FormulationTypeRequestModel request);
        Task<bool> DeleteAsync(int id);
    }
}