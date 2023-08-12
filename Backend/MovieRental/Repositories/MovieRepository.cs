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

        public List<Movie> GetMoviesWithRating(decimal? minRating, decimal? maxRating)
        {
            // var filter = Builders<Movie>.Filter.And(
            //     Builders<Movie>.Filter.Lte(h => h.Rating, maxRating),
            //     Builders<Movie>.Filter.Gte(h => h.Rating, minRating)
            // );
            
            var filterBuilder = Builders<Movie>.Filter;
            var filter = Builders<Movie>.Filter.Empty;

            if (minRating.HasValue)
            {
                filter &= filterBuilder.Gte(h => h.Rating, minRating);
            }

            if (maxRating.HasValue)
            {
                filter &= filterBuilder.Lte(h => h.Rating, maxRating);
            }

            return _collection.Find(filter).ToList();
        }

        public List<Movie> GetMoviesWithStringInTitle(string title)
        {
            var filter = Builders<Movie>.Filter.Regex(h => h.Title, title);
            return _collection.Find(filter).ToList();
        }

        public List<Movie> GetMoviesBetweenYears(int? startYear, int? endYear)
        {
            var filterBuilder = Builders<Movie>.Filter;
            var filter = Builders<Movie>.Filter.Empty;

            if (startYear.HasValue)
            {
                filter &= filterBuilder.Gte(h => h.ReleaseDate, startYear.Value);
            }

            if (endYear.HasValue)
            {
                filter &= filterBuilder.Lte(h => h.ReleaseDate, endYear.Value);
            }

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