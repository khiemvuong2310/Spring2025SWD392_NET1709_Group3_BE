using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Mapping
{
    public class SkinTypeMapping : Profile
    {
        public SkinTypeMapping()
        {
            CreateMap<SkinType, SkinTypeResponseModel>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDelete));
            CreateMap<SkinTypeRequestModel,SkinType>();

            CreateMap<RoutineSkinType, RoutineOrUserSkinTypeResponse>()
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Percentage * 100));

            CreateMap<UserSkinType, RoutineOrUserSkinTypeResponse>()
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Percentage * 100));
        }
    }
}
