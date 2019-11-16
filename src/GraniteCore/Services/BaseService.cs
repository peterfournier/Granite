using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public abstract class BaseService<TDtoModel, TEntity, TPrimaryKey> : IBaseService<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        protected virtual IBaseRepository<TDtoModel, TEntity, TPrimaryKey> Repository { get; private set; }
        protected virtual IGraniteMapper Mapper { get; private set; }

        public BaseService(
            IBaseRepository<TDtoModel, TEntity, TPrimaryKey> repository,
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

        public virtual Task<TDtoModel> Create(TDtoModel dtoModel)
        {
            return Repository.Create(dtoModel);
        }

        public virtual Task Delete(TPrimaryKey id)
        {
            return Repository.Delete(id);
        }

        public virtual Task Update(TPrimaryKey id, TDtoModel dtoModel)
        {
            return Repository.Update(id, dtoModel);
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
