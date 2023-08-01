using MongoDB.Driver;
using MongoDB.Bson;
using MovieRental.Models;

namespace MovieRental.Repositories
{
    public class MovieRepository
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
    }
}