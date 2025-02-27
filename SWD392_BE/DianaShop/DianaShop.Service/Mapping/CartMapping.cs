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
    public class CartMapping : Profile
    {

        public CartMapping()
        {
            CreateMap<CartRequestModel, Cart>();
            //CreateMap<Cart, CartResponseModel>()
            //    .ForMember(dest => dest.CartID, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => new ProductResponseCart
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Description = p.Description,
            //        Price = p.Price
            //    })));

            CreateMap<Product, ProductResponseCart>();
        }
    }
}
