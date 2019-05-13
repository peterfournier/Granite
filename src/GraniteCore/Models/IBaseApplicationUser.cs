namespace GraniteCore
{
    public interface IBaseApplicationUser<TPrimaryKey> : IBaseIdentityModel<TPrimaryKey>
    {        
        string FirstName { get; set; }
        string LastName { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }
}
