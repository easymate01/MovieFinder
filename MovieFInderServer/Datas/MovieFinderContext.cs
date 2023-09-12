using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Models;

namespace MovieFInderServer.Datas
{
    public class MovieFinderContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:moviefinderdb.database.windows.net,1433;Initial Catalog=movieDb;Persist Security Info=False;User ID=GULMATAN;Password=itL156!\\*AK8;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public DbSet<SavedMovie> SavedMovies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.LikedMovies)
                .WithMany(m => m.LikedByUsers);

            modelBuilder.Entity<SavedMovie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies);

        }
    }
}
