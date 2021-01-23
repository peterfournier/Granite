using System;
using GraniteCore;

namespace MyCars.Domain.Entities
{
    // GraniteCore install
    public class CustomerEntity : UserBasedEntityModel<Guid, IUser<string>, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }
    }
}
