using Granite.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Granite
{
    public interface IBaseService<TDtoModel, TEntity, TPrimaryKey, TUserID>
        where TDtoModel : class, IUserBasedDto<TPrimaryKey, TUserID>, new()
        where TEntity : class, IBaseEntity<TPrimaryKey, TUserID>, new()
    {
        IQueryable<TDtoModel> GetAll();
        Task<TDtoModel> Create(TDtoModel entity, TUserID userId);
        Task Delete(TPrimaryKey id, TUserID userId);
        Task Update(TPrimaryKey id, TDtoModel entity, TUserID userId);
        Task<TDtoModel> GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
