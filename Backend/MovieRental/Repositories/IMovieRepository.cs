using MovieRental.Models;

namespace MovieRental.Repositories
{
    public interface IMovieRepository
    {
        List<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        List<Movie> FilterMoviesV1(string category, decimal? minRating, decimal? maxRating, int? startYear, int? endYear);
        Task InsertMovie<T>(Movie movie);
        Task UpdateMovie<T>(Movie movie);
        Task DeleteMovie<T>(Movie movie);
        List<Movie> GetMoviesByCategory(string category);
        List<Movie> GetMoviesFromYear(int fromYear);
        List<Movie> GetMoviesToYear(int toYear);
        List<Movie> GetMoviesWithRating(decimal? minRating, decimal? maxRating);
        List<Movie> GetMoviesWithStringInTitle(string title);
        List<Movie> GetMoviesBetweenYears(int? startYear, int? endYear);
    }
}