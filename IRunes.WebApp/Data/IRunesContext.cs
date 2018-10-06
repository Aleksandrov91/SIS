namespace IRunes.WebApp.Data
{
    using IRunes.WebApp.Models;
    using Microsoft.EntityFrameworkCore;

    public class IRunesContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=IRunes;Integrated Security=true")
                .UseLazyLoadingProxies();
        }
    }
}
