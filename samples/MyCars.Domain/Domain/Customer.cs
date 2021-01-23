using System;
using GraniteCore;

namespace MyCars.Domain
{
    public class Customer : UserBaseDomainModel<Guid, IUser<string>, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }
    }
}
