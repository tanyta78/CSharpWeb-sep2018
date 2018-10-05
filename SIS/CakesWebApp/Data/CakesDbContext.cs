namespace CakesWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class CakesDbContext:DbContext
    {
        public CakesDbContext()
        {
            
        }

        public CakesDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ServerConfig.ConnectionString);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

    }
}
