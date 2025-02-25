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
    public class CommentMapping : Profile
    {
        public CommentMapping()
        {
            CreateMap<CommentRequestModel, Comment>()
    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
    .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<CommentUpdateRequestModel, Comment>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Comment, CommentResponseModel>().ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
        }
    }
}
