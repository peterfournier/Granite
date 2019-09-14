namespace GraniteCore
{
    public interface IUserBasedDto<TPrimaryKey, TUserPrimaryKey, TUser> : IUserBasedModel<TPrimaryKey, TUserPrimaryKey, TUser>
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {

    }
}
