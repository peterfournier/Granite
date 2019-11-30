using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IBaseService<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : IBaseIdentityModel<TPrimaryKey>, new()
    {
        IQueryable<TDtoModel> GetAll();
        Task<TDtoModel> Create(TDtoModel dtoModel);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TDtoModel dtoModel);
        Task<TDtoModel> GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
