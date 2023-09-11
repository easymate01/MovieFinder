using Microsoft.AspNetCore.Mvc;
using MovieFInderServer.Models;
using MovieFInderServer.Models.DTOs;
using MovieFInderServer.Services.Genres;
using MovieFInderServer.Services.Movies;

namespace MovieFInderServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveMovieController : Controller
    {
        private readonly ILogger<SaveMovieController> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;

        public SaveMovieController(ILogger<SaveMovieController> logger, IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }

        [HttpGet]
        [Route("api/movies")]
        public async Task<ActionResult<IEnumerable<SavedMovie>>> GetAllSavedMovies()
        {
            try
            {
                var movies = await _movieRepository.GetAllAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving saved movies.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Route("api/savemovie")]
        public async Task<ActionResult<SavedMovie>> SaveMovie(SavedMovieDTO savingmovie)
        {
            var movie = await _movieRepository.GetMovieByName(savingmovie.Title);
            try
            {
                if (movie == null)
                {
                    movie = new SavedMovie
                    {
                        MovieId = savingmovie.MovieId,
                        Title = savingmovie.Title,
                        ImageUrl = savingmovie.ImageUrl,
                        Owerview = savingmovie.Overview,
                        ReleaseDate = savingmovie.ReleaseDate,
                        Genres = new List<Genre>()

                    };


                    foreach (var genreId in savingmovie.GenreIds)
                    {
                        // Check if the genre exists in the database
                        var genre = await _genreRepository.GetGenreByIdAsync(genreId);

                        if (genre == null)
                        {
                            // Create a new genre if it doesn't exist
                            genre = new Genre
                            {
                                GenreId = genreId,
                                MovieId = savingmovie.MovieId,
                            };

                            // Add the new genre to the database
                            await _genreRepository.AddGenre(genre);
                        }
                    }

                    await _movieRepository.Add(movie);
                    Console.WriteLine($"Movie: {savingmovie.Title} added to Database...");
                    return Ok(movie);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving saved movies.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

            return Ok(movie);
        }
    }
}
