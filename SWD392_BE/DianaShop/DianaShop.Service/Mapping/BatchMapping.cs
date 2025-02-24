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
    public class BatchMapping : Profile
    {
        public BatchMapping()
        {
            CreateMap<ProductBatch, BatchResponseModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Product.Status ? "Active" : "Inactive"));
            CreateMap<BatchRequestModel, ProductBatch>();
        }
    }
}
