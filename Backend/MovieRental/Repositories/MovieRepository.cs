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

        public List<Movie> FilterMoviesV1(string category, decimal? minRating, decimal? maxRating, int? startYear,
            int? endYear)
        {
            var filter = Builders<Movie>.Filter.Empty;
            
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                filter &= Builders<Movie>.Filter.Eq(r => r.MovieCategory, category);
            }

            if (minRating != null)
            {
                filter &= Builders<Movie>.Filter.Gte(r => r.Rating, minRating);
            }

            if (maxRating != null)
            {
                filter &= Builders<Movie>.Filter.Lte(r => r.Rating, maxRating);
            }

            if (startYear != null)
            {
                filter &= Builders<Movie>.Filter.Gte(r => r.ReleaseDate, startYear);
            }
            
            if (endYear != null)
            {
                filter &= Builders<Movie>.Filter.Lte(r => r.ReleaseDate, endYear);
            }

            return _collection.Find(filter).ToList();
        }
        
        public Movie GetMovieById(int id)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.Id, id);

            return _collection.Find(filter).FirstOrDefault();
        }

        public List<Movie> GetMoviesWithStringInTitle(string title)
        {
            var filter = Builders<Movie>.Filter.Regex(h => h.Title, title);
            return _collection.Find(filter).ToList();
        }

        public async Task InsertMovie<T>(Movie movie)
        {
            await _collection.InsertOneAsync(movie);
        }

        public async Task UpdateMovie<T>(Movie movie)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.Id, movie.Id);
            await _collection.ReplaceOneAsync(filter, movie);
        }

        public async Task DeleteMovie<T>(Movie movie)
        {
            var filter = Builders<Movie>.Filter.Eq(h => h.Id, movie.Id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}