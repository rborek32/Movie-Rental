using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieRental.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MovieCategory { get; set; }
        public int ReleaseDate { get; set; }
        public decimal Rating { get; set; }
    }
}