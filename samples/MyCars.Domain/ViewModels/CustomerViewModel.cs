using GraniteCore;
using MyCars.Domain.Entities;
using System;

namespace MyCars.Domain.ViewModels
{
    public class CustomerViewModel : UserBaseDomainModel<Guid, ApplicationUser, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }
    }
}
