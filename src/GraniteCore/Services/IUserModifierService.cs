namespace GraniteCore.Services
{
    public interface IUserModifierService<TUser, TUserPrimaryKey>
        where TUser : class, IBaseApplicationUser<TUserPrimaryKey>
    {
        TUser User { get; }
        void SetUser(TUser user);
    }
}
