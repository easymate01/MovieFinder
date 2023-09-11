using MovieFInderServer.Models;

namespace MovieFInderServer.Services.Movies
{
    public interface IMovieRepository
    {
        Task<IEnumerable<SavedMovie>> GetAllAsync();

        Task<SavedMovie> GetMovieByName(string movieName);

        Task Add(SavedMovie movie);
        Task Delete(SavedMovie movie);
    }
}
