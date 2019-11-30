using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCars.Areas.Identity;
using MyCars.Domain.Models;

namespace MyCars.Data
{
    // GraniteCore install <GraniteCoreApplicationUser, IdentityRole, string>
    public class ApplicationDbContext : IdentityDbContext<GraniteCoreApplicationUser, IdentityRole, string>
    {
        public DbSet<CarEntity> Cars { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomerEntity>()
                .HasOne(x => x.CreatedByUser as GraniteCoreApplicationUser);

            builder.Entity<CustomerEntity>()
                .HasOne(x => x.LastModifiedByUser as GraniteCoreApplicationUser);
        }
    }
}
