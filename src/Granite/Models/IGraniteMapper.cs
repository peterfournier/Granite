using System.Linq;

namespace GraniteCore
{
    public interface IGraniteMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
        IQueryable<TDestination> Map<TSource, TDestination>(IQueryable<TSource> source);
        //IList<TDestination> Map<TSource, TDestination>(IList<TSource> source);
    }

}