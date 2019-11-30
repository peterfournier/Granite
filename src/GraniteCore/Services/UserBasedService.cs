using System.Threading.Tasks;

namespace GraniteCore
{
    public abstract class UserBasedService<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> : BaseService<TDtoModel, TEntity, TPrimaryKey>, IUserBaseService<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : IBaseIdentityModel<TPrimaryKey>, new()        
    {
        protected new virtual IUserBasedRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> Repository { get; private set; }

        public IBaseApplicationUser<TUserPrimaryKey> User { get; private set; }

        public UserBasedService(
            IUserBasedRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> repository,
            IGraniteMapper mapper
            ) : base(repository, mapper)
        {
            Repository = repository;
        }

        public virtual void SetUser(IBaseApplicationUser<TUserPrimaryKey> user)
        {
            if (user != null)
            {
                User = user;
            }
        }

        public new virtual Task<TDtoModel> Create(TDtoModel dtoModel)
        {
            return Repository.Create(dtoModel, User);
        }

        public new virtual Task Delete(TPrimaryKey id)
        {
            return Repository.Delete(id, User);
        }

        public new virtual Task Update(TPrimaryKey id, TDtoModel dtoModel)
        {
            return Repository.Update(id, dtoModel, User);
        }
    }
}
