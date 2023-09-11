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
            try
            {
                var existingMovie = await _movieRepository.GetMovieByName(savingmovie.Title);

                if (existingMovie == null)
                {
                    var newMovie = new SavedMovie
                    {
                        Title = savingmovie.Title,
                        ImageUrl = savingmovie.ImageUrl,
                        Owerview = savingmovie.Overview,
                        ReleaseDate = savingmovie.ReleaseDate,
                        Genres = new List<Genre>() // Létrehoz egy üres Genre lista
                    };

                    await _movieRepository.Add(newMovie);

                    // The movie exists, create the join with the genres
                    foreach (var genreId in savingmovie.GenreIds)
                    {
                        var genre = await _genreRepository.GetGenreByIdAsync(genreId);
                        if (genre == null)
                        {
                            genre = new Genre
                            {
                                MovieId = newMovie.Id,
                                GenreId = genreId
                            };
                            await _genreRepository.AddGenre(genre);
                        }
                        newMovie.Genres.Add(genre);
                    }

                    await _movieRepository.Update(newMovie);

                    Console.WriteLine($"Movie: {savingmovie.Title} added to Database.");
                    return Ok(newMovie);
                }
                else
                {
                    // The movie exists, so only the join need to be created
                    foreach (var genreId in savingmovie.GenreIds)
                    {
                        var genre = await _genreRepository.GetGenreByIdAsync(genreId);
                        if (genre == null)
                        {
                            genre = new Genre
                            {
                                MovieId = existingMovie.Id,
                                GenreId = genreId
                            };
                            await _genreRepository.AddGenre(genre);
                        }
                        existingMovie.Genres.Add(genre);
                    }

                    await _movieRepository.Update(existingMovie);

                    Console.WriteLine($"Genres added to existing movie: {existingMovie.Title}");
                    return Ok(existingMovie);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}
