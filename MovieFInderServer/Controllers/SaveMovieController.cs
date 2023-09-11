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
        [HttpPost]
        [Route("api/savemovie")]
        public async Task<ActionResult<SavedMovie>> SaveMovie(SavedMovieDTO savingmovie)
        {
            try
            {
                // Ellenőrizd, hogy a film már létezik az adatbázisban a cím alapján
                var existingMovie = await _movieRepository.GetMovieByName(savingmovie.Title);

                if (existingMovie == null)
                {
                    // Ha a film még nem létezik, hozz létre egy újat
                    var newMovie = new SavedMovie
                    {
                        Title = savingmovie.Title,
                        ImageUrl = savingmovie.ImageUrl,
                        Owerview = savingmovie.Overview,
                        ReleaseDate = savingmovie.ReleaseDate,
                        Genres = new List<Genre>() // Létrehoz egy üres Genre lista
                    };

                    // Hozz létre és mentsd el a filmet az adatbázisban
                    await _movieRepository.Add(newMovie);

                    // Most, hogy a film létezik, hozz létre és mentsd el a kapcsolatot a műfajokkal
                    foreach (var genreId in savingmovie.GenreIds)
                    {
                        var genre = await _genreRepository.GetGenreByIdAsync(genreId);
                        if (genre == null)
                        {
                            // Ha a műfaj még nem létezik, hozz létre egy újat
                            genre = new Genre
                            {
                                MovieId = newMovie.Id, // A film azonosítójával lásd el
                                GenreId = genreId
                            };
                            await _genreRepository.AddGenre(genre);
                        }
                        // Adj hozzá a műfajt a filmhez
                        newMovie.Genres.Add(genre);
                    }

                    // Mentsd el újra a filmet, hogy a kapcsolatok is mentésre kerüljenek
                    await _movieRepository.Update(newMovie);

                    Console.WriteLine($"Movie: {savingmovie.Title} added to Database...");
                    return Ok(newMovie);
                }
                else
                {
                    // A film már létezik az adatbázisban, ezért csak a kapcsolatot kell hozzáadni a műfajokkal
                    foreach (var genreId in savingmovie.GenreIds)
                    {
                        var genre = await _genreRepository.GetGenreByIdAsync(genreId);
                        if (genre == null)
                        {
                            // Ha a műfaj még nem létezik, hozz létre egy újat
                            genre = new Genre
                            {
                                MovieId = existingMovie.Id, // A meglévő film azonosítójával lásd el
                                GenreId = genreId
                            };
                            await _genreRepository.AddGenre(genre);
                        }
                        // Adj hozzá a műfajt a meglévő filmhez
                        existingMovie.Genres.Add(genre);
                    }

                    // Mentsd el a meglévő filmet, hogy a kapcsolatok is mentésre kerüljenek
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
