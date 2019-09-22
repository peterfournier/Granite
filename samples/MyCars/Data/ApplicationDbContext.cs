using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCars.Areas.Identity;
using MyCars.Domain.Models;
using MyCars.Domain.ViewModels;

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
        public DbSet<MyCars.Domain.ViewModels.CustomerViewModel> CustomerViewModel { get; set; }
    }
}
