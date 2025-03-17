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
    public class SkincareRoutineMapping : Profile
    {
        public SkincareRoutineMapping()
        {
            CreateMap<SkincareRoutine, SkincareRoutineResponseModel>()
                .ForMember(dest => dest.RoutineSteps, opt => opt.MapFrom(src => src.RoutineSteps))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDelete))
                .ForMember(dest => dest.SkinTypes, opt => opt.MapFrom(src => src.RoutineSkinTypes.Select(rs => rs.SkinType.Name)));
            CreateMap<RoutineStep, RoutineStepResponseModel>();

            CreateMap<SkincareRoutineRequestModel, SkincareRoutine>()
                .ForMember(dest => dest.RoutineSteps, opt => opt.MapFrom(src => src.RoutineSteps));
            CreateMap<RoutineStepRequestModel, RoutineStep>();



        }
    }
}
