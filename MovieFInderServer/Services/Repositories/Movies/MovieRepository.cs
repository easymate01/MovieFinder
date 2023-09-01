using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Models;

namespace MovieFInderServer.Services.Repositories.Movies
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

        public async Task Delete(SavedMovie city)
        {
            using var dbContext = new MovieFinderContext();
            dbContext.Remove(city);
            await dbContext.SaveChangesAsync();
        }

    }
}
