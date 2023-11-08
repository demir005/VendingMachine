using Microsoft.EntityFrameworkCore;
using VendingMachine.Models;

namespace VendingMachine.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
