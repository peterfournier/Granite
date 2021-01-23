namespace GraniteCore.MVC.ViewModels
{
    public class UserViewModel<TUserPrimaryKey> : IUser<TUserPrimaryKey>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public TUserPrimaryKey ID { get; set; }
    }
}
