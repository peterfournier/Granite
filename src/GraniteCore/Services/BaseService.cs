using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public abstract class BaseService<TDtoModel, TEntity, TPrimaryKey, TUserID> : IBaseService<TDtoModel, TEntity, TPrimaryKey, TUserID>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        protected readonly IBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserID> Repository;
        protected readonly IGraniteMapper Mapper;

        public BaseService(
            IBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserID> repository,
            IGraniteMapper mapper
            )
        {
            Mapper = mapper;
            Repository = repository;
        }

        public virtual IQueryable<TDtoModel> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual Task<TDtoModel> Create(TDtoModel entity, TUserID userId)
        {
            return Repository.Create(entity, userId);
        }

        public virtual Task Delete(TPrimaryKey id, TUserID userId)
        {
            return Repository.Delete(id, userId);
        }

        public virtual Task Update(TPrimaryKey id, TDtoModel entity, TUserID userId)
        {
            return Repository.Update(id, entity, userId);
        }

        public virtual Task<TDtoModel> GetById(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            return Repository.GetById(id, includeProperties);
        }
    }
}
