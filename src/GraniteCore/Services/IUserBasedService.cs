using GraniteCore.Services;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBaseService<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey> : IUserModifierService<TUser, TUserPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : IBaseIdentityModel<TPrimaryKey>, new()
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {
        Task<TDtoModel> Create(TDtoModel dtoModel);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TDtoModel dtoModel);
    }
}
