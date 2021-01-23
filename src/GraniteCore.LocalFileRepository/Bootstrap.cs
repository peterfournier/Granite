using Microsoft.Extensions.DependencyInjection;

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
