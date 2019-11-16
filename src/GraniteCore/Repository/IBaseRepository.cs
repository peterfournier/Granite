using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IBaseRepository<TDtoModel, TEntity ,TPrimaryKey>        
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        IQueryable<TDtoModel> GetAll();
        Task<TDtoModel> Create(TDtoModel entity);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TDtoModel entity);
        Task<TDtoModel> GetByID(TPrimaryKey id);
        Task<TDtoModel> GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
