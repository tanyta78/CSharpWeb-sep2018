namespace IRunesWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class IRunesDbContext:DbContext
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(ServerConfig.ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<AlbumTrack> AlbumsTracks { get; set; }

    }
}
