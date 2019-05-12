﻿using Microsoft.Extensions.DependencyInjection;

namespace Granite.EntityFrameworkCore
{
    public static class Bootstrap
    {
        public static void AddGraniteEntityFrameworkCore(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<,,,>), typeof(BaseRepository<,,,>));
        }
    }
}
