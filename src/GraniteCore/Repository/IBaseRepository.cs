using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IBaseRepository<TBaseEntityModel ,TPrimaryKey>
        where TBaseEntityModel : IBaseIdentityModel<TPrimaryKey>, new()
    {
        IQueryable<TBaseEntityModel> GetAll();
        Task<TBaseEntityModel> Create(TBaseEntityModel dtoModel);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TBaseEntityModel dtoModel);
        Task<TBaseEntityModel> GetByID(TPrimaryKey id);
        Task<TBaseEntityModel> GetByID(TPrimaryKey id, params Expression<Func<TBaseEntityModel, object>>[] includeProperties);
    }
}
