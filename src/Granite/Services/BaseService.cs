using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Granite.Models;

namespace Granite
{
    public abstract class BaseService<TDtoModel, TEntity, TPrimaryKey, TUserID> : IBaseService<TDtoModel, TEntity, TPrimaryKey, TUserID>
        where TDtoModel : class, IUserBasedDto<TPrimaryKey,TUserID>, new()
        where TEntity : class, IBaseEntity<TPrimaryKey, TUserID>, new()
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

        public IQueryable<TDtoModel> GetAll()
        {
            return Repository.GetAll();
        }

        public Task<TDtoModel> Create(TDtoModel entity, TUserID userId)
        {
            return Repository.Create(entity, userId);
        }

        public Task Delete(TPrimaryKey id, TUserID userId)
        {
            return Repository.Delete(id, userId);
        }

        public Task Update(TPrimaryKey id, TDtoModel entity, TUserID userId)
        {
            return Repository.Update(id, entity, userId);
        }

        public Task<TDtoModel> GetById(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            return Repository.GetById(id, includeProperties);
        }
    }
}
