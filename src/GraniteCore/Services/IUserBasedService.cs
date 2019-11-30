using GraniteCore.Services;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBaseService<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> : IUserModifierService<TUserPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : IBaseIdentityModel<TPrimaryKey>, new()        
    {
        Task<TDtoModel> Create(TDtoModel dtoModel);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TDtoModel dtoModel);
    }
}
