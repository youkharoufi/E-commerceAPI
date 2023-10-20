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

    }
}
