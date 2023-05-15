using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Watch.Models;

namespace Watch.DataAccess
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        //data is a base class
        {

       }
       public DbSet<Category> WatchCategories { get; set; }
       public DbSet<CoverType> CoverTypes { get; set; }
       public DbSet<Product> Products { get; set; }
    }
}
