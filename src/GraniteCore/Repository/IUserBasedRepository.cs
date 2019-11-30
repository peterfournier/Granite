using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBasedRepository<TDtoModel, TEntity ,TPrimaryKey, TUserPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : IBaseIdentityModel<TPrimaryKey>, new()        
    {
        Task<TDtoModel> Create(TDtoModel dtoModel, IBaseApplicationUser<TUserPrimaryKey> user);
        Task Delete(TPrimaryKey id, IBaseApplicationUser<TUserPrimaryKey> user);
        Task Update(TPrimaryKey id, TDtoModel dtoModel, IBaseApplicationUser<TUserPrimaryKey> user);
    }
}
