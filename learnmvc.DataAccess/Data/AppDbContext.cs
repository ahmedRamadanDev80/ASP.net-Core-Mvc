using learnmvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace learnmvc.DataAccess
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
                
        }
        public DbSet<Course> courses { get; set; } 
        public DbSet<Category>  categories { get; set; } 
        public DbSet<CoverType> coverTypes { get; set; } 
        public DbSet<Product> products { get; set; } 
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 
        public DbSet<Company> companies { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
