using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MovieRental.Models;

namespace MovieRental.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _collection;

        public MovieRepository(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<Movie>(collectionName);
        }

        public List<Movie> GetAllMovies()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }

        public Movie GetMovieById(int id)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.MovieId, id);
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<Movie> GetMoviesByCategory(string category)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.MovieCategory, category);
            return _collection.Find(filter).ToList();
        }

        public List<Movie> GetMoviesFromYear(int fromYear)
        {
            var filter = Builders<Movie>.Filter.Gte(h => h.ReleaseDate, fromYear);
            return _collection.Find(filter).ToList();
        }

        public List<Movie> GetMoviesToYear(int toYear)
        {
            var filter = Builders<Movie>.Filter.Lte(h => h.ReleaseDate, toYear);
            return _collection.Find(filter).ToList();
        }

        public List<Movie> GetMoviesWithRating(decimal minRating, decimal maxRating)
        {
            var filter = Builders<Movie>.Filter.And(
                Builders<Movie>.Filter.Lte(h => h.Rating, maxRating),
                Builders<Movie>.Filter.Gte(h => h.Rating, minRating)
            );
            return _collection.Find(filter).ToList();
        }

        public async Task InsertMovie<T>(Movie movie)
        {
            await _collection.InsertOneAsync(movie);
        }

        public async Task UpdateMovie<T>(Movie movie)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.Id, movie.Id);
            _collection.ReplaceOne(filter, movie);
        }

        public async Task DeleteMovie<T>(Movie movie)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.MovieId, movie.MovieId);
            _collection.DeleteOne(filter);
        }
    }
}