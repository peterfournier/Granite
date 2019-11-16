using System;
using System.Threading.Tasks;

namespace GraniteCore
{
    public abstract class UserBasedService<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey> : BaseService<TDtoModel, TEntity, TPrimaryKey>, IUserBaseService<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
        where TUser : class, IBaseApplicationUser<TUserPrimaryKey>
    {
        protected new virtual IUserBasedRepository<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey> Repository { get; private set; }

        public TUser User { get; private set; }

        public UserBasedService(
            IUserBasedRepository<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey> repository,
            IGraniteMapper mapper
            ) : base(repository, mapper)
        {
            Repository = repository;
        }

        public virtual void SetUser(TUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public new virtual Task<TDtoModel> Create(TDtoModel entity)
        {
            return Repository.Create(entity, User);
        }

        public new virtual Task Delete(TPrimaryKey id)
        {
            return Repository.Delete(id, User);
        }

        public new virtual Task Update(TPrimaryKey id, TDtoModel entity)
        {
            return Repository.Update(id, entity, User);
        }
    }
}
