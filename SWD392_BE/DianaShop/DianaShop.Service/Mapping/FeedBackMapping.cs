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
    public class FeedBackMapping : Profile
    {
        public FeedBackMapping()
        {
            // FeedBackRequestModel -> FeedBack
            CreateMap<FeedBackRequestModel, FeedBack>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating)) // Ánh xạ Rating từ Request
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ReplyOn, opt => opt.MapFrom(src => src.ReplyOn));
            CreateMap<FeedBack, FeedbackResponse>();
            // FeedBack -> FeedBackResponseModel
            CreateMap<FeedBack, FeedBackResponseModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating)) // Ánh xạ Rating từ FeedBack
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ReplyOn, opt => opt.MapFrom(src => src.ReplyOn))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))  // Ánh xạ thông tin User
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product)); // Ánh xạ thông tin Product

        }
    }
}
