using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Repositories;

namespace MovieRental.Controllers
{
    [Route("api/movies/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieRepository _movieRepository;

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
    }
}
