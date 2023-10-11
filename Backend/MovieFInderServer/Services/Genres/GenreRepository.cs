using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Datas;
using MovieFInderServer.Models;

namespace MovieFInderServer.Services.Genres
{
    public class GenreRepository : IGenreRepository
    {
        public async Task<Genre> GetGenreByIdAsync(int genreId)
        {
            using var dbContext = new MovieFinderContext();
            return await dbContext.Genres.SingleOrDefaultAsync(g => g.GenreId == genreId);
        }

        public async Task AddGenre(Genre genre)
        {
            using var dbContext = new MovieFinderContext();
            dbContext.Genres.Add(genre);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Genre genre)
        {
            using var dbContext = new MovieFinderContext();
            dbContext.Genres.Update(genre);
            await dbContext.SaveChangesAsync();
        }

    }
}
