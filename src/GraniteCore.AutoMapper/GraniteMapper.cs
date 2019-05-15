using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace GraniteCore.AutoMapper
{
    public class GraniteMapper : IGraniteMapper
    {
        public GraniteMapper()
        {

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
