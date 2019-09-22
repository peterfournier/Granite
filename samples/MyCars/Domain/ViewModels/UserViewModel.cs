using GraniteCore;

namespace MyCars.Domain.ViewModels
{
    // Granite install
    public class UserViewModel : IBaseApplicationUser<string>
    {
        public string FirstName { get; set; }
        public string FullName => $"{LastName}, {FirstName}";
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ID { get; set; }
        //public string Id { get; set; }
    }
}
