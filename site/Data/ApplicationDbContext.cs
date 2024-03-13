using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Site.Models;

namespace Site.Data
{
    // to make this not identity remove the Identity
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Stub> Stubs { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Country> Countries { get; set; }
        
    }
}
