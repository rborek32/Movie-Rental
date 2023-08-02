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

        public List<Movie> GetMoviesByCategory(string category)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.MovieCategory, category);
            return _collection.Find(filter).ToList();
        }

        public List<Movie> GetMoviesFromYear(int fromYear)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.ReleaseDate, fromYear);
            //return  _collection.Find()
        }

        public List<Movie> GetMoviesToYear(int toYear)
        {
            throw new NotImplementedException();
        }

        public List<Movie> GetMoviesWithRating(float minRating, float maxRating)
        {
            throw new NotImplementedException();
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
            var filter = Builders<Movie>.Filter.Eq(h => h.Id, movie.Id);
            _collection.DeleteOne(filter);
        }
    }
}