using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Models;

namespace MovieFInderServer.Datas
{
    public class MovieFinderContext : DbContext
    {

        public DbSet<SavedMovie> SavedMovies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:moviefinderdb.database.windows.net,1433;Initial Catalog=MovieFinderDb;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


    }
}
