namespace GraniteCore.Services
{
    public interface IUserModifierService<TUserPrimaryKey>
    {
        IUser<TUserPrimaryKey> User { get; }
        void SetUser(IUser<TUserPrimaryKey> user);
    }
}
