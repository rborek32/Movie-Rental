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

        public async Task AddReservation<T>(Reservation reservation)
        {
            await _collection.InsertOneAsync(reservation);
        }

        public async Task RemoveReservation(Reservation reservation)
        {
            var filter = Builders<Reservation>.Filter.Eq(h => h.Id, reservation.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public Task RemoveReservation(int Id)
        {
            throw new NotImplementedException();
        }

        public Task GetAmountOfReservations(string Title)
        {
            throw new NotImplementedException();
        }
        
        public Reservation GetReservationById(int id)
        {
            var filter = Builders<Reservation>.Filter.Eq(h => h.Id, id);
            return _collection.Find(filter).FirstOrDefault();
        }
    }
}
