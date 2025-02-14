using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping() 
        {
            // Ánh xạ từ CategoryRequestModel -> Category (Entity)
            CreateMap<CategoryRequestModel, Category>();

            // Ánh xạ từ Category (Entity) -> CategoryResponseModel
            CreateMap<Category, CategoryResponseModel>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => new ProductResponseModel
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList()));
        }
    }
}
