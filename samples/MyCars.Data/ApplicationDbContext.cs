using Microsoft.EntityFrameworkCore;
using MyCars.Domain.Models;

namespace MyCars.Data
{
    // GraniteCore install <GraniteCoreApplicationUser, IdentityRole, string>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CarEntity> Cars { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomerEntity>()
                .HasOne(x => x.CreatedByUser as ApplicationUser);

            builder.Entity<CustomerEntity>()
                .HasOne(x => x.LastModifiedByUser as ApplicationUser);
        }
    }
}
