﻿using System.Runtime.InteropServices.JavaScript;

namespace ReservationService.Models
{
    public class Reservation
    {
        public int? Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MovieTitle { get; set; }
    }
}
