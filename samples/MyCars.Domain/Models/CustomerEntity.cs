using System;
using GraniteCore;

namespace MyCars.Domain.Models
{
    // GraniteCore install
    public class CustomerEntity : UserBasedModel<Guid, ApplicationUser, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }
    }
}
