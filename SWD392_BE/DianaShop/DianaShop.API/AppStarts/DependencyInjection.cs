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
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 2)); // Adjust the version to match your MySQL version

                options.UseMySql(connectionString, serverVersion);
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
            services.AddTransient<IPackagingService, PackagingService>();
            services.AddTransient<ISkinTypeTestService, SkinTypeTestService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IFormulationTypeService, FormulationTypeService>();
            services.AddTransient<IIngredientService, IngredientService>();
            services.AddTransient<IBatchService, BatchService>();
            services.AddTransient<ITagService, TagService>();

            // Thêm AutoMapper vào dịch vụ
            services.AddAutoMapper(typeof(CategoryMapping));  
            services.AddAutoMapper(typeof(PackagingMapping));
            services.AddAutoMapper(typeof(ProductMapping));
            services.AddAutoMapper(typeof(FeedBackMapping));
            services.AddAutoMapper(typeof(CartMapping));
        }
    }
}
