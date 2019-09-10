using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GraniteCore.AutoMapper
{
    public static class Bootstrap
    {
        public static void AddGraniteAutoMapper(
            this IServiceCollection services,
            Action<IMapperConfigurationExpression> config)
        {
            var graniteCoreMapper = new GraniteCoreMapper(config);
            services.AddSingleton(typeof(IGraniteMapper), graniteCoreMapper);
        }
        public static void AddGraniteAutoMapper(
            this IServiceCollection services,
            MapperConfigurationExpression config
            )
        {
            var graniteCoreMapper = new GraniteCoreMapper(config);
            services.AddSingleton(typeof(IGraniteMapper), graniteCoreMapper);
        }
    }
}
