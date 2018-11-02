namespace GameStoreWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AppDbContext:DbContext
    {
        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(ServerConfig.ConnectionString)
                    .UseLazyLoadingProxies();;
            }
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<UserGame> UserGames { get; set; }

        //public DbSet<ChannelTag> ChannelTags { get; set; }

        //public DbSet<UsersInChannel> UsersInChannels { get; set; }

    }
}
