namespace GraniteCore
{
    public interface IUserBasedDto<TPrimaryKey, TUser, TUserPrimaryKey> : IUserBasedModel<TPrimaryKey, TUser, TUserPrimaryKey>
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {

    }
}
