using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IBaseService<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey>        
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        IQueryable<TDtoModel> GetAll();
        Task<TDtoModel> Create(TDtoModel entity, TUserPrimaryKey userId);
        Task Delete(TPrimaryKey id, TUserPrimaryKey userId);
        Task Update(TPrimaryKey id, TDtoModel entity, TUserPrimaryKey userId);
        Task<TDtoModel> GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
