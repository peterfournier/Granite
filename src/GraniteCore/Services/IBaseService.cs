using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IBaseService<TBaseDomainModel, TBaseEntityModel, TPrimaryKey>
        where TBaseDomainModel : IBaseIdentityModel<TPrimaryKey>, new()
        where TBaseEntityModel : IEntity<TPrimaryKey>, new()
    {
        IQueryable<TBaseDomainModel> GetAll();
        Task<TBaseDomainModel> Create(TBaseDomainModel dtoModel);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TBaseDomainModel dtoModel);
        Task<TBaseDomainModel> GetByID(TPrimaryKey id);
        Task<TBaseDomainModel> GetByID(TPrimaryKey id, params Expression<Func<TBaseEntityModel, object>>[] includeProperties);
    }
}
