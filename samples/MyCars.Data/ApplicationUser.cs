using GraniteCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCars.Areas.Identity // changed namespace to save myself from fixing all the views in Identity
{
    // GraniteCore install
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<string>, IUser<string>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [NotMapped]
        public string ID { get => base.Id; set => base.Id = value; }
    }
}
