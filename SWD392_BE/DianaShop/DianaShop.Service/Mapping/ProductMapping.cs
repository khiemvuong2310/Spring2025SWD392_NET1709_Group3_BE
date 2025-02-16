using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductResponseModel>()
            .ForMember(dest => dest.FormulationType, opt => opt.MapFrom(src => src.FormulationType.Texture))
            .ForMember(dest => dest.Packaging, opt => opt.MapFrom(
                src =>  $"{(src.Packaging.IsRefillable ? "Refillable" : "Non-refillable")} {src.Packaging.Size} {src.Packaging.Material} {src.Packaging.Type}"))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name));
        }
    }
}
