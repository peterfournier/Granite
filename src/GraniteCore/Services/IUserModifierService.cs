namespace GraniteCore.Services
{
    public interface IUserModifierService<TUser, TUserPrimaryKey>
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {
        TUser User { get; }
        void SetUser(TUser user);
    }
}
