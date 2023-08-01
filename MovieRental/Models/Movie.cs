using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieRental.Models
{
    public class Movie
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int MovieId { get; set; }

        public string Title { get; set; }

        public string MovieCategory { get; set; }
    }
}