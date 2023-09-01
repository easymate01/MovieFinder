using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Models.Entities;

namespace MovieFInderServer.Models
{
    public class MovieFinderContext : DbContext
    {

        public DbSet<SavedMovie> SavedMovies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=SavedMoviesApi;User Id=sa;Password=yourStrong(!)Password;Encrypt=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SavedMovie>()
                .HasKey(m => m.Id); // Define the primary key for SavedMovie

            modelBuilder.Entity<SavedMovie>()
                .HasIndex(m => m.Id)
                .IsUnique();

            // Configure the Genre entity
            modelBuilder.Entity<Genre>()
                .HasKey(g => g.Id); // Define the primary key for Genre

            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Id)
                .IsUnique();


            modelBuilder.Entity<MovieGenre>() // Configure the join entity MovieGenre
                .HasKey(j => new { j.MovieId, j.GenreId }); // Define the composite primary key

            modelBuilder.Entity<MovieGenre>()
                .HasOne(j => j.Movie)
                .WithMany(m => m.Genres)
                .HasForeignKey(j => j.MovieId); // Configure the MovieId foreign key

            modelBuilder.Entity<MovieGenre>()
                .HasOne(j => j.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(j => j.GenreId); // Configure the GenreId foreign key
        }


    }
}
