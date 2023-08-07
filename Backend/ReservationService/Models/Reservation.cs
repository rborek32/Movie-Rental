using System.Runtime.InteropServices.JavaScript;

namespace ReservationService.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string Movie { get; set; }
    }
}
