using DianaShop.Data;
using DianaShop.Repository.Repositories;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.Mapping;
using DianaShop.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Clients;

namespace DianaShop.API.AppStarts
{
    public static class DependencyInjection
    {
        public static void InstallService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true; ;
                options.LowercaseQueryStrings = true;
            });

            services.AddSingleton<ITwilioRestClient>(new TwilioRestClient("ACCOUNT_SID", "AUTH_TOKEN"));

            //Add Scoped
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IFeedBackService, FeedBackService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPackagingService, PackagingService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IPackagingService, PackagingService>();
            services.AddScoped<ISkinTypeTestService, SkinTypeTestService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IVoucherStorageService, VoucherStorageService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IPackagingService, PackagingService>();
            services.AddScoped<ISkinTypeTestService, SkinTypeTestService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IFormulationTypeService, FormulationTypeService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IBatchService, BatchService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICartProductService, CartProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ISkinTypeService, SkinTypeService>();
            services.AddScoped<ISkincareRoutineService, SkincareRoutineService>();
            services.AddScoped<IBlobService, BlobService>();
            //services.AddScoped<IJwtService, JwtService>();

            // Thêm AutoMapper vào dịch vụ
            services.AddAutoMapper(typeof(CategoryMapping));  
            services.AddAutoMapper(typeof(PackagingMapping));
            services.AddAutoMapper(typeof(ProductMapping));
            services.AddAutoMapper(typeof(FeedBackMapping));
            services.AddAutoMapper(typeof(CartMapping));
            services.AddAutoMapper(typeof (OrderMapping));
            services.AddAutoMapper(typeof(OrderDetailMapping));
            services.AddAutoMapper(typeof(PaymentMapping));
            services.AddAutoMapper(typeof(UserMapping));
            services.AddAutoMapper(typeof(UserRoleMapping));
            services.AddAutoMapper(typeof(VoucherMapping));
            services.AddAutoMapper(typeof(CommentMapping));
        }
    }
}
