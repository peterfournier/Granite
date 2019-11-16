﻿using GraniteCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCars.Areas.Identity
{
    // GraniteCore install
    [Table("AspNetUsers")]
    public class GraniteCoreApplicationUser : IdentityUser, IBaseApplicationUser<string>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public string ID { get => Id; set => Id = value; }
    }
}
