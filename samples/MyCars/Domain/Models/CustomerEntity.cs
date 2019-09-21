using System;
using GraniteCore;
using MyCars.Areas.Identity;

namespace MyCars.Domain.Models
{
    // GraniteCore install
    public class CustomerEntity : UserBasedModel<Guid, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }

        #region Nav Props
        public new ApplicationUser CreatedByUser { get; set; }
        public new ApplicationUser LastModifiedByUser { get; set; }
        #endregion
    }
}
