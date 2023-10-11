using MovieFInderServer.Models;

namespace MovieFInderServer.Services.Genres
{
    public interface IGenreRepository
    {
        Task<Genre> GetGenreByIdAsync(int genreId);
        Task AddGenre(Genre genre);

        Task Update(Genre genre);
        // Other genre-related methods as needed
    }
}
