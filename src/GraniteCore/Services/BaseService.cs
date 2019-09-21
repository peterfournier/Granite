using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public abstract class BaseService<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> : IBaseService<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey>        
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        protected readonly IBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> Repository;
        protected readonly IGraniteMapper Mapper;

        public BaseService(
            IBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> repository,
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

        public virtual Task<TDtoModel> Create(TDtoModel entity, TUserPrimaryKey userPrimaryKey)
        {
            return Repository.Create(entity, userPrimaryKey);
        }

        public virtual Task Delete(TPrimaryKey id, TUserPrimaryKey userPrimaryKey)
        {
            return Repository.Delete(id, userPrimaryKey);
        }

        public virtual Task Update(TPrimaryKey id, TDtoModel entity, TUserPrimaryKey userPrimaryKey)
        {
            return Repository.Update(id, entity, userPrimaryKey);
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
