using GraniteCore.Services;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBaseService<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey> : IUserModifierService<TUser, TUserPrimaryKey>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
        where TUser : class, IBaseApplicationUser<TUserPrimaryKey>
    {
        Task<TDtoModel> Create(TDtoModel dtoModel);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TDtoModel dtoModel);
    }
}
