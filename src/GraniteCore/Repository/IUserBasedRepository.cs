using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBasedRepository<TBaseEntityModel ,TPrimaryKey, TUserPrimaryKey> : IBaseRepository<TBaseEntityModel, TPrimaryKey>
        where TBaseEntityModel : IBaseIdentityModel<TPrimaryKey>, new()
    {
        Task<TBaseEntityModel> Create(TBaseEntityModel entityModel, IBaseApplicationUser<TUserPrimaryKey> user);
        Task Delete(TPrimaryKey id, IBaseApplicationUser<TUserPrimaryKey> user);
        Task Update(TPrimaryKey id, TBaseEntityModel entityModel, IBaseApplicationUser<TUserPrimaryKey> user);
    }
}
