using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieRental.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int MovieId { get; set; }
        public string Title { get; set; }
        public string MovieCategory { get; set; }
        public int ReleaseDate { get; set; }
        public decimal Rating { get; set; }
    }
}