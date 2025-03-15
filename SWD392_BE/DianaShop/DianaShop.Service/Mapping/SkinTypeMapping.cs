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
            CreateMap<SkinType, SkinTypeResponseModel>();
            CreateMap<SkinTypeRequestModel,SkinType>();
        }
    }
}
