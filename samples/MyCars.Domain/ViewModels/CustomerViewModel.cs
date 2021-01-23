using GraniteCore;
using System;

namespace MyCars.Domain.ViewModels
{
    public class CustomerViewModel : UserBaseDomainModel<Guid, IUser<string>, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }
    }
}
