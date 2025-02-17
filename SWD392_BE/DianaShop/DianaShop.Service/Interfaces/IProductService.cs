using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseModel> AddProductAsync(ProductRequestModel model);
        Task<IEnumerable<ProductResponseModel>> GetAllProductsAsync();
        Task<ProductResponseModel?> GetProductByIdAsync(int id);
    }
}