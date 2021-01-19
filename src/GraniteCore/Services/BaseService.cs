using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraniteCore
{
    public abstract class BaseService<TBaseDomainModel, TBaseEntityModel, TPrimaryKey> : IBaseService<TBaseDomainModel, TBaseEntityModel, TPrimaryKey>
                where TBaseDomainModel : IBaseIdentityModel<TPrimaryKey>, new()
                where TBaseEntityModel : IEntity<TPrimaryKey>, new()
    {
        protected virtual IBaseRepository<TBaseEntityModel, TPrimaryKey> Repository { get; private set; }
        protected virtual IGraniteMapper Mapper { get; private set; }

        public BaseService(
            IBaseRepository<TBaseEntityModel, TPrimaryKey> repository,
            IGraniteMapper mapper
            )
        {
            Mapper = mapper;
            Repository = repository;
        }

        public virtual IQueryable<TBaseDomainModel> GetAll()
        {
            return Mapper.Map<TBaseEntityModel, TBaseDomainModel>(Repository.GetAll());
        }

        public async virtual Task<TBaseDomainModel> Create(TBaseDomainModel domainModel)
        {
            var entity = Mapper.Map<TBaseDomainModel, TBaseEntityModel>(domainModel);

            return Mapper.Map<TBaseEntityModel, TBaseDomainModel>(await Repository.Create(entity));
        }

        public virtual Task Delete(TPrimaryKey id)
        {
            return Repository.Delete(id);
        }

        public virtual Task Update(TPrimaryKey id, TBaseDomainModel domainModel)
        {
            var entity = Mapper.Map<TBaseDomainModel, TBaseEntityModel>(domainModel);

            return Repository.Update(id, entity);
        }

        public async virtual Task<TBaseDomainModel> GetByID(
            TPrimaryKey id
            )
        {
            return Mapper.Map<TBaseEntityModel, TBaseDomainModel>(await Repository.GetByID(id));
        }

        public async virtual Task<TBaseDomainModel> GetByID(
            TPrimaryKey id,
            params Expression<Func<TBaseEntityModel, object>>[] includeProperties
            )
        {
            return Mapper.Map<TBaseEntityModel, TBaseDomainModel>(await Repository.GetByID(id, includeProperties));
        }
    }
}
