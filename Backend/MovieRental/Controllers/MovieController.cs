using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Models;
using MovieRental.Repositories;

namespace MovieRental.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MongoDB");
            _movieRepository = new MovieRepository(connectionString, "Movie", "Movies");
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var movies = _movieRepository.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("{category}")]
        public IActionResult GetMovieByCategory(string category)
        {
            var movies = _movieRepository.GetMoviesByCategory(category);
            return Ok(movies);
        }

        [HttpGet("getMoviesBetweenYears")]
        public IActionResult GetMoviesBetweenYears([FromQuery] int? startYear, int? endYear)
        {
            var movies = _movieRepository.GetMoviesBetweenYears(startYear, endYear);
            return Ok(movies);
        }
        
        [HttpGet("getMoviesWithRating")]
        public IActionResult GetMoviesWithRating([FromQuery] decimal? minRating, decimal? maxRating)
        {
            var movies = _movieRepository.GetMoviesWithRating(minRating, maxRating);
            return Ok(movies);
        }

        [HttpGet("titleContains")]
        public IActionResult GetMoviesThatContainRegexInTitle([FromQuery] string title)
        {
            var movies = _movieRepository.GetMoviesWithStringInTitle(title);
            return Ok(movies);
        }

        [HttpPost("addMovie")]
        public async Task<IActionResult> PostMovie([FromBody] Movie movie)
        {
            try
            {
                await _movieRepository.InsertMovie<Movie>(movie);
                return Ok("Movie inserted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to insert movie. Error: {ex.Message}");
            }
        }

        [HttpPut("updateMovie")]
        public async Task<IActionResult> PutMovie([FromBody] Movie movie)
        {
            try
            {
                await _movieRepository.UpdateMovie<Movie>(movie);
                return Ok("Movie updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update movie. Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var movie = _movieRepository.GetMovieById(id);
                if (movie == null)
                {
                    return NotFound("Movie not found.");
                }

                await _movieRepository.DeleteMovie<Movie>(movie);
                return Ok("Movie deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete movie. Error: {ex.Message}");
            }
        }
    }
}
