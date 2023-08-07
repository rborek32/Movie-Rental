using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReservationService.Models;
using ReservationService.Repositories;

namespace ReservationService.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MongoDB");
            _reservationRepository = new ReservationRepository(connectionString, "Movie", "Reservations");
        }
        [HttpGet("/")]
        public IActionResult GetAllReservations()
        {
            return Ok("Works");
        }
        
        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Reservation reservation)
        {
            try
            {
                await _reservationRepository.AddReservation<Reservation>(reservation);
                return Ok("Movie inserted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to insert movie. Error: {ex.Message}");
            }
        }

        [HttpDelete("/delete/{Id}")]
        public IActionResult DeleteReservation(int Id)
        {
            try
            {
                var reservation = _reservationRepository.GetReservationById(Id);
                if (reservation == null)
                {
                    return NotFound("Reservation not found.");
                }

                _reservationRepository.RemoveReservation(reservation);
                return Ok("Movie deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete movie. Error: {ex.Message}");
            }
        }
    }
}
