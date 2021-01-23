using Microsoft.EntityFrameworkCore;
using MyCars.Domain.Entities;
using GraniteCore;
using MyCars.Areas.Identity;

namespace MyCars.Data
{
    // GraniteCore install <ApplicationUser, IdentityRole, string>
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

            builder.Entity<ApplicationUser>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ApplicationUser>()
                .HasKey(x => x.Id);


            builder.Entity<CustomerEntity>()
                .HasOne(x => x.CreatedByUser as ApplicationUser);

            builder.Entity<CustomerEntity>()
                .HasOne(x => x.LastModifiedByUser as ApplicationUser);
        }
    }
}
