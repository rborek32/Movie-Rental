using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Models;
using MovieRental.Repositories;

namespace MovieRental.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        //private readonly MovieRepository _movieRepository;
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
    }
}
