using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq;

namespace GraniteCore.AutoMapper
{
    public class GraniteCoreMapper : IGraniteMapper
    {
        public GraniteCoreMapper(Action<IMapperConfigurationExpression> config)
        {
            Mapper.Initialize(config);
        }
        public GraniteCoreMapper(MapperConfigurationExpression config)
        {
            Mapper.Initialize(config);
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        public virtual IQueryable<TDestination> Map<TSource, TDestination>(IQueryable<TSource> source)
        {

            return source.ProjectTo<TDestination>();
        }
    }
}
