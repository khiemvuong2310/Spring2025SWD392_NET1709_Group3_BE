using DianaShop.Service.Interfaces;
using DianaShop.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IPackagingService, PackagingService>();
            service.AddTransient<IProductService, ProductService>();
            return service;
        }
    }
}
