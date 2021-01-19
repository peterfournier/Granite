using GraniteCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCars.Domain.Entities
{
    [Table("AspNetUsers")] // Mapping to the AspNetCoreIdentity Schema
    public class ApplicationUser : BaseApplicationUser<string>
    {
        [NotMapped]
        public new string ID { get => Id; set => Id = value; }

        [Key] // this is a special case because we're using AspNetCoreIdentity Id vs ID
        public string Id { get; set; }
    }
}
