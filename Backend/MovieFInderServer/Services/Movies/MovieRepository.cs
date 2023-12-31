﻿using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Datas;
using MovieFInderServer.Models;

namespace MovieFInderServer.Services.Movies
{
    public class MovieRepository : IMovieRepository
    {

        public async Task<IEnumerable<SavedMovie>> GetAllAsync()
        {
            using var dbContext = new MovieFinderContext();
            return await dbContext.SavedMovies.ToListAsync();
        }

        public async Task<SavedMovie> GetMovieByName(string movieName)
        {
            using var dbContext = new MovieFinderContext();
            return await dbContext.SavedMovies
                .SingleOrDefaultAsync(c => c.Title == movieName);
        }
        public async Task Add(SavedMovie movie)
        {
            using var dbContext = new MovieFinderContext();
            dbContext.Add(movie);
            await dbContext.SaveChangesAsync();
        }
        public async Task Update(SavedMovie movie)
        {
            using var dbContext = new MovieFinderContext();
            dbContext.SavedMovies.Update(movie);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            using var dbContext = new MovieFinderContext();
            var movieToDelete = await dbContext.SavedMovies.FirstOrDefaultAsync(movie => movie.Id == id);
            dbContext.Remove(movieToDelete);
            await dbContext.SaveChangesAsync();
        }

    }
}
