using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientResponseModel>> GetAllAsync();
        Task<IngredientResponseModel?> GetByIdAsync(int id);
        Task<IngredientResponseModel> CreateAsync(IngredientRequestModel request);
        Task<IngredientResponseModel?> UpdateAsync(int id, IngredientRequestModel request);
        Task<bool> DeleteAsync(int id);
    }
}
