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
    public class PackagingMapping : Profile
    {
        public PackagingMapping()
        {
            // Ánh xạ từ PackagingRequestModel -> Packaging (Entity)
            CreateMap<PackagingRequestModel, Packaging>();
            // Ánh xạ từ Packaging (Entity) -> PackagingResponseModel
            CreateMap<Packaging, PackagingResponseModel>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        }
    }
}
