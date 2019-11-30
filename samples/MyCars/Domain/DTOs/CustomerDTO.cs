using System;
using GraniteCore;

namespace MyCars.Domain.DTOs
{
    public class CustomerDTO : UserBasedDto<Guid, IBaseApplicationUser<string>, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }

        //public new GraniteCoreApplicationUser CreatedByUser { get; set; }
        //public new GraniteCoreApplicationUser LastModifiedByUser { get; set; }
    }
}
