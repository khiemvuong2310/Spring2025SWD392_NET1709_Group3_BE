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
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductRequestModel, Product>();
            CreateMap<Product, Product>();
            CreateMap<Product, ProductResponseModel>()
            .ForMember(dest => dest.FormulationType, opt => opt.MapFrom(src => src.FormulationType.Texture))
            .ForMember(dest => dest.Packaging, opt => opt.MapFrom(
                src =>  $"{(src.Packaging.IsRefillable ? "Refillable" : "Non-refillable")} {src.Packaging.Size} {src.Packaging.Material} {src.Packaging.Type}"))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.ProductBatches.Where(pb => pb.Status == true && !pb.IsDelete).Sum(pb => pb.Quantity)))
            .ForMember(dest => dest.ProductBatchIds, opt => opt.MapFrom(src => src.ProductBatches.Select(pb => pb.Id).ToList()))
            .ForMember(dest => dest.ProductTags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag.Name).ToList()))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.Image.Base64Image).ToList()))
            .ForMember(dest => dest.ProductIngredients, opt => opt.MapFrom(src => src.ProductIngredients.Select(pi => pi.Ingredient.Name).ToList()))
            .ForMember(dest => dest.ProductSkinTypes, opt => opt.MapFrom(src => src.ProductSkinTypes.Select(ps => ps.SkinType.Name).ToList()))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDelete));
        }
    }
}
