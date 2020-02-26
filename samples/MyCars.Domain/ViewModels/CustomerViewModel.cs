using GraniteCore;
using MyCars.Domain.Models;
using System;

namespace MyCars.Domain.ViewModels
{
    public class CustomerViewModel : UserBasedModel<Guid, ApplicationUser, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }
    }
}
