using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TheHiddenSquirrel.Models.Category> Category { get; set; } = default!;
        public DbSet<TheHiddenSquirrel.Models.Product> Product { get; set; } = default!;
        public DbSet<TheHiddenSquirrel.Models.CartItem> CartItem { get; set; } = default!;
        public DbSet<Order> Order { get; set;  }
        public DbSet<OrderDetail> OrderDetail { get; set; }  
    }
}
