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
    public class TagMapping : Profile
    {
        public TagMapping()
        {
            CreateMap<Tag, TagResponseModel>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Product.Name).ToList()))
                .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.PostTags.Select(pt => pt.Post.Title).ToList()));
            CreateMap<TagRequestModel, Tag>();
        }
    }
}
