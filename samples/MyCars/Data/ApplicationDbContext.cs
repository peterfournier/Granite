using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCars.Domain.Models;

namespace MyCars.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<CarEntity> Cars { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
