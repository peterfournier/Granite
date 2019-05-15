using Microsoft.Extensions.DependencyInjection;

namespace GraniteCore.AutoMapper
{
    public static class Bootstrap
    {
        public static void AddGraniteAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IGraniteMapper, GraniteMapper>();
        }
    }
}
