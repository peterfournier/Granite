using GraniteCore.Services;
using System.Threading.Tasks;

namespace GraniteCore
{
    public interface IUserBaseService<TBaseDomainModel, TBaseEntityModel, TPrimaryKey, TUserPrimaryKey> : IUserModifierService<TUserPrimaryKey>
        where TBaseDomainModel : IBaseIdentityModel<TPrimaryKey>, new()
        where TBaseEntityModel : IEntity<TPrimaryKey>, new()
    {
        Task<TBaseDomainModel> Create(TBaseDomainModel dtoModel);
        Task Delete(TPrimaryKey id);
        Task Update(TPrimaryKey id, TBaseDomainModel dtoModel);
    }
}
