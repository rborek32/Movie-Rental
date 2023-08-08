﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult GetAllReservations()
        {
            var reservations = _reservationRepository.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("/amount/movieTitle")]
        public async Task<ActionResult<long>> GetAmountOfReservations(string title)
        {
            long count = await _reservationRepository.GetAmountOfReservations(title);
            return count;
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Reservation reservation)
        {
            try
            {
                await _reservationRepository.AddReservation<Reservation>(reservation);
                return Ok("Reservation inserted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to Reservation movie. Error: {ex.Message}");
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

        [HttpDelete("/delete/many/{movieTitle}")]
        public IActionResult DeleteReservationsByTitle(string movieTitle)
        {
            try
            {
                long deletedCount = _reservationRepository.RemoveReservationsByTitle(movieTitle);

                if (deletedCount == 0)
                {
                    return NotFound("No reservations found for the given movie title.");
                }

                return Ok($"Deleted {deletedCount} reservations with the given movie title.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete reservations. Error: {ex.Message}");
            }
        }
    }
}
