using Microsoft.AspNetCore.Mvc;
using MovieFInderServer.Models;
using MovieFInderServer.Models.DTOs;
using MovieFInderServer.Services.Genres;
using MovieFInderServer.Services.Movies;
using MovieFInderServer.Services.Users;

namespace MovieFInderServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveMovieController : Controller
    {
        private readonly ILogger<SaveMovieController> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUserService _userService;
        public SaveMovieController(ILogger<SaveMovieController> logger, IMovieRepository movieRepository, IGenreRepository genreRepository, IUserService userService)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _userService = userService;
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
        public async Task<ActionResult<SavedMovie>> SaveMovie(SavedMovieDTO savingmovie, int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var existingMovie = await _movieRepository.GetMovieByName(savingmovie.Title);

                if (existingMovie == null)
                {
                    var newMovie = new SavedMovie
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
                        var genre = await _genreRepository.GetGenreByIdAsync(genreId);

                        if (genre == null)
                        {
                            genre = new Genre
                            {
                                GenreId = genreId,
                                MovieId = newMovie.MovieId
                            };
                            await _genreRepository.AddGenre(genre);
                        }

                        newMovie.Genres.Add(genre);
                    }

                    user.LikedMovies.Add(newMovie);
                    await _movieRepository.Add(newMovie);
                    await _userService.UpdateUser(user);

                    Console.WriteLine($"Movie: {savingmovie.Title} added to Database.");
                    return Ok(newMovie);
                }
                else
                {
                    // Ha a film már létezik, csak a join táblákat kell létrehozni
                    foreach (var genreId in savingmovie.GenreIds)
                    {
                        var genre = await _genreRepository.GetGenreByIdAsync(genreId);

                        if (genre == null)
                        {
                            // Ha a genre nem létezik, létrehozzuk
                            genre = new Genre
                            {
                                GenreId = genreId,
                                MovieId = existingMovie.MovieId
                            };
                            await _genreRepository.AddGenre(genre);
                        }

                        existingMovie.Genres.Add(genre);
                    }

                    // A join táblákat mentjük
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
