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
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            // Mapping User -> UserResponseModel
            CreateMap<User, UserResponseModel>()
                .ForMember(dest => dest.SkinTypeName, opt => opt.MapFrom(src => src.SkinType.Name))
                .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart))
                .ForMember(dest => dest.VoucherStorage, opt => opt.MapFrom(src => src.VoucherStorage))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name))) // Chỉ lấy tên role
                .ForMember(dest => dest.FeedBacks, opt => opt.MapFrom(src => src.FeedBacks))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders));

            // Mapping UserCreateRequest -> User
            CreateMap<UserCreateRequest, User>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore()) // Không tạo giỏ hàng ngay khi đăng ký
                .ForMember(dest => dest.VoucherStorage, opt => opt.Ignore()) // Không có voucher khi tạo mới
                .ForMember(dest => dest.FeedBacks, opt => opt.Ignore()) // Không có feedback khi tạo mới
                .ForMember(dest => dest.Orders, opt => opt.Ignore()) // Không có đơn hàng khi tạo mới
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));// Ánh xạ Status
        }
    }

}
