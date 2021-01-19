using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraniteCore.LocalFileRepository
{
    public static class Bootstrap
    {
        public static void AddGraniteEntityFrameworkCore(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<,>), typeof(LocalFileRepository<,>));
        }
    }
}
