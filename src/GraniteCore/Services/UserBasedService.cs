using System.Threading.Tasks;

namespace GraniteCore
{
    public abstract class UserBasedService<TBaseDomainModel, TBaseEntityModel, TPrimaryKey, TUserPrimaryKey> : BaseService<TBaseDomainModel, TBaseEntityModel, TPrimaryKey>, IUserBaseService<TBaseDomainModel, TBaseEntityModel, TPrimaryKey, TUserPrimaryKey>
                where TBaseDomainModel : IBaseIdentityModel<TPrimaryKey>, new()
                where TBaseEntityModel : IEntity<TPrimaryKey>, new()
    {
        protected new virtual IUserBasedRepository<TBaseEntityModel, TPrimaryKey, TUserPrimaryKey> Repository { get; private set; }

        public IUser<TUserPrimaryKey> User { get; private set; }

        public UserBasedService(
            IUserBasedRepository<TBaseEntityModel, TPrimaryKey, TUserPrimaryKey> repository,
            IGraniteMapper mapper
            ) : base(repository, mapper)
        {
            Repository = repository;
        }

        public virtual void SetUser(IUser<TUserPrimaryKey> user)
        {
            if (user != null)
            {
                User = user;
            }
        }

        public async new virtual Task<TBaseDomainModel> Create(TBaseDomainModel domainModel)
        {
            var entity = Mapper.Map<TBaseDomainModel, TBaseEntityModel>(domainModel);

            return Mapper.Map<TBaseEntityModel, TBaseDomainModel>(await Repository.Create(entity, User));
        }

        public new virtual Task Delete(TPrimaryKey id)
        {
            return Repository.Delete(id, User);
        }

        public new virtual Task Update(TPrimaryKey id, TBaseDomainModel domainModel)
        {
            var entity = Mapper.Map<TBaseDomainModel, TBaseEntityModel>(domainModel);

            return Repository.Update(id, entity, User);
        }
    }
}
