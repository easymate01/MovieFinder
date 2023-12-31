﻿using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Models;

namespace MovieFInderServer.Datas
{
    public class MovieFinderContext : DbContext
    {

        public DbSet<SavedMovie> SavedMovies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:moviefinderdb.database.windows.net,1433;Initial Catalog=MovieFinderDb;Persist Security Info=False;User ID=GULMATAN;Password=itL156!\\*AK8;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


    }
}
