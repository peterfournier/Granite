using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCars.Areas.Identity;
using MyCars.Areas.Identity.Data;

namespace MyCars.Data
{
    // GraniteCore install <GraniteCoreApplicationUser, IdentityRole, string>
    public class AspNetCoreIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        
        public AspNetCoreIdentityDbContext(DbContextOptions<AspNetCoreIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        
        }
    }
}
