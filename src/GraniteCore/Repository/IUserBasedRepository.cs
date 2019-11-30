using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBasedRepository<TDtoModel, TEntity ,TPrimaryKey, TUser, TUserPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : IBaseIdentityModel<TPrimaryKey>, new()
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {
        Task<TDtoModel> Create(TDtoModel dtoModel, TUser user);
        Task Delete(TPrimaryKey id, TUser user);
        Task Update(TPrimaryKey id, TDtoModel dtoModel, TUser user);
    }
}
