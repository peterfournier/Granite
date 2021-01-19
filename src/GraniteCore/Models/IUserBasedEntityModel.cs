namespace GraniteCore
{
    public interface IUserBasedEntityModel<TPrimaryKey, TUser, TUserPrimaryKey> : IUserBasedDomainModel<TPrimaryKey, TUser, TUserPrimaryKey>
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {

    }
}
