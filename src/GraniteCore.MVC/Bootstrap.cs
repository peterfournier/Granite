using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace GraniteCore.MVC
{
    public static class Bootstrap
    {
        public static void AddGraniteEntityFrameworkCore(this IServiceCollection services)
        {
            //services.AddScoped<ILogger, Logger<>>
        }
    }
}
