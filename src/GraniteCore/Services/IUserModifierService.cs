namespace GraniteCore.Services
{
    public interface IUserModifierService<TUserPrimaryKey>        
    {
        IBaseApplicationUser<TUserPrimaryKey> User { get; }
        void SetUser(IBaseApplicationUser<TUserPrimaryKey> user);
    }
}
