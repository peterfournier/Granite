using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IBaseRepository<TDtoModel, TEntity ,TPrimaryKey, TUserID>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        IQueryable<TDtoModel> GetAll();
        Task<TDtoModel> Create(TDtoModel entity, TUserID userId);
        Task Delete(TPrimaryKey id, TUserID userId);
        Task Update(TPrimaryKey id, TDtoModel entity, TUserID userId);
        Task<TDtoModel> GetById(TPrimaryKey id);
        Task<TDtoModel> GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
