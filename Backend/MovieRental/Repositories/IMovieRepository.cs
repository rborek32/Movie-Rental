using MovieRental.Models;

namespace MovieRental.Repositories
{
    public interface IMovieRepository
    {
        List<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        List<Movie> GetMoviesWithStringInTitle(string title);
        Task InsertMovie<T>(Movie movie);
        Task UpdateMovie<T>(Movie movie);
        Task DeleteMovie<T>(Movie movie);
    }
}