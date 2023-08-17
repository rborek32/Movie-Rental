using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Models;
using MovieRental.Repositories;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
        
        [HttpGet("test")]
        public IActionResult Test()
        {
            string solutionDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(solutionDir, "movies.json");
            return Ok("Movie service works" + filePath);
        }
        
        [HttpGet("filterMovies")]
        public IActionResult FilterMovies([FromQuery] string? title, string? category, decimal? minRating, decimal? maxRating, int? startYear, int? endYear)
        {
            var movies = _movieRepository.GetAllMovies();

            if (title != null)
            {
                movies = movies.Where(movie => movie.Title == title).ToList();
            }

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                movies = movies.Where(movie => movie.MovieCategory == category).ToList();
            }

            if (minRating != null)
            {
                movies = movies.Where(movie => movie.Rating >= minRating).ToList();
            }

            if (maxRating != null)
            {
                movies = movies.Where(movie => movie.Rating <= maxRating).ToList();
            }

            if (startYear != null)
            {
                movies = movies.Where(movie => movie.ReleaseDate >= startYear).ToList();
            }
            
            if (endYear != null)
            {
                movies = movies.Where(movie => movie.ReleaseDate <= endYear).ToList();
            }

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
        
        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeMovies()
        {
            try
            {
                string solutionDir = Directory.GetCurrentDirectory();
                string filePath = Path.Combine(solutionDir, "movies.json");

                if (!System.IO.File.Exists(filePath))
                {
                    return BadRequest("movies.json file not found.");
                }

                string jsonData = await System.IO.File.ReadAllTextAsync(filePath);

                var movies = JsonConvert.DeserializeObject<List<Movie>>(jsonData);

                foreach (var movie in movies)
                {
                    await _movieRepository.InsertMovie<Movie>(movie);
                }

                return Ok("Movies initialized successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to initialize movies. Error: {ex.Message}");
            }
        }
    }
}
