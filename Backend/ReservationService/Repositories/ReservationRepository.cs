using System.Collections.Immutable;
using MongoDB.Bson;
using MongoDB.Driver;
using ReservationService.Models;

namespace ReservationService.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IMongoCollection<Reservation> _collection;

        public ReservationRepository(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<Reservation>(collectionName);
        }

        public List<Reservation> GetAllReservations()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }

        public async Task AddReservation<T>(Reservation reservation)
        {
            long count = await GetCountOfReservations();
            reservation.Id = (int)count + 1;

            await _collection.InsertOneAsync(reservation);
        }

        public async Task RemoveReservation(Reservation reservation)
        {
            var filter = Builders<Reservation>.Filter.Eq(h => h.Id, reservation.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<long> GetAmountOfReservations(string title)
        {
            var filter = Builders<Reservation>.Filter.Eq(h=>h.MovieTitle, title);
            return await _collection.CountDocumentsAsync(filter);
        }

        public Reservation GetReservationById(int id)
        {
            var filter = Builders<Reservation>.Filter.Eq(h => h.Id, id);
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<Reservation> GetReservationsByTitle(string title)
        {
            var filter = Builders<Reservation>.Filter.Eq(r => r.MovieTitle, title);
            return _collection.Find(filter).ToList();
        }
        
        public async Task<long> GetCountOfReservations()
        {
            var filter = Builders<Reservation>.Filter.Empty;
            return await _collection.CountDocumentsAsync(filter);
        }

        public long RemoveReservationsByTitle(string movieTitle)
        {
            var filter = Builders<Reservation>.Filter.Eq(r => r.MovieTitle, movieTitle);
            var result = _collection.DeleteMany(filter);
            return result.DeletedCount;
        }

        public async Task<bool> CheckReservation(Reservation reservation)
        {
            var filter = Builders<Reservation>.Filter.And(
                Builders<Reservation>.Filter.Eq(r => r.MovieTitle, reservation.MovieTitle),
                Builders<Reservation>.Filter.Or(
                    Builders<Reservation>.Filter.And(
                        Builders<Reservation>.Filter.Lte(r => r.StartDate, reservation.EndDate),
                        Builders<Reservation>.Filter.Gte(r => r.EndDate, reservation.StartDate)
                    ),
                    Builders<Reservation>.Filter.And(
                        Builders<Reservation>.Filter.Gte(r => r.StartDate, reservation.StartDate),
                        Builders<Reservation>.Filter.Lte(r => r.StartDate, reservation.EndDate)
                    )
                )
            );
            var existingReservation = await _collection.Find(filter).FirstOrDefaultAsync();

            return existingReservation != null;
        }
        
        public async Task UpdateReservation<T>(Reservation reservation)
        {
            var filter = Builders<Reservation>.Filter.Eq(h => h.Id, reservation.Id);
            _collection.ReplaceOne(filter, reservation);
        }
    }
}
