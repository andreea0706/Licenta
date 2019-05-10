using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacturaOnline.Models
{
    public class AppDbContext : IdentityDbContext<Partner>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<FacturaOnline.Models.Partner> Partners { get; set; }
        public DbSet<FacturaOnline.Models.Customer> Customers { get; set; }
        public DbSet<FacturaOnline.Models.Product> Products { get; set; }
        public DbSet<FacturaOnline.Models.Category> Categories { get; set; }
    }
}
