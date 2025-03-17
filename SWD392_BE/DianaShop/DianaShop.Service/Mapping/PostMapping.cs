using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Mapping
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostResponseModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ? "Published" : "Draft"))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.PostImages.Select(pi => pi.Image.Base64Image)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags.Select(pt => pt.Tag.Name)))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDelete));
            CreateMap<PostRequestModel, Post>();
        }
    }
}
