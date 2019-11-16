using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBasedRepository<TDtoModel, TEntity ,TPrimaryKey, TUser, TUserPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
        where TUser : class, IBaseApplicationUser<TUserPrimaryKey>
    {
        Task<TDtoModel> Create(TDtoModel entity, TUser user);
        Task Delete(TPrimaryKey id, TUser user);
        Task Update(TPrimaryKey id, TDtoModel entity, TUser user);
    }
}
