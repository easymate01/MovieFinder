using Microsoft.AspNetCore.Mvc;
using MovieFInderServer.Models;
using MovieFInderServer.Services.Repositories.Movies;

namespace MovieFInderServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveMovieController : Controller
    {
        private readonly ILogger<SaveMovieController> _logger;
        private readonly IMovieRepository _movieRepository;

        public SaveMovieController(ILogger<SaveMovieController> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
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
        public async Task<ActionResult<SavedMovie>> SaveMovie(SavedMovie savingmovie)
        {
            var movie = await _movieRepository.GetMovieByName(savingmovie.Title);
            try
            {
                if (movie == null)
                {
                    movie = savingmovie;
                    await _movieRepository.Add(movie);
                    //need to add genres to this returning movie!!
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
