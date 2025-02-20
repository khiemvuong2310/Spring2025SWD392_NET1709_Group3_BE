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
    public class FormulationTypeMapping : Profile
    {
        public FormulationTypeMapping()
        {
            CreateMap<FormulationTypeRequest, FormulationType>();
            CreateMap<FormulationType, FormulationTypeResponse>()
                .ForMember(dest => dest.ProductNames, opt => opt.MapFrom(src => src.Products.Select(p => p.Name).ToList()));
        }
    }
}
