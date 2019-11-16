using System;
using GraniteCore;
using MyCars.Areas.Identity;

namespace MyCars.Domain.Models
{
    // GraniteCore install
    public class CustomerEntity : UserBasedModel<Guid, GraniteCoreApplicationUser, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }

        #region Nav Props
        public override GraniteCoreApplicationUser CreatedByUser { get; set; }
        public override GraniteCoreApplicationUser LastModifiedByUser { get; set; }
        #endregion
    }
}
