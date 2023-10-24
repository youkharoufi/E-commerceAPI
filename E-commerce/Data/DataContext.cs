using E_commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {

        public DataContext(DbContextOptions options): base(options)
        {
            
        }

    public DbSet<Product> Products { get; set; }

    public DbSet<Cart> cart { get; set; }

    public DbSet<CartItems> cartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cart>()
                .HasOne(c=> c.User)
                .WithMany()
                .HasForeignKey(cart => cart.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
        }

    }
}
