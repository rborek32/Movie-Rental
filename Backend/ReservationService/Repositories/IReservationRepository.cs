using ReservationService.Models;

namespace ReservationService.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservations();
        Task AddReservation<T>(Reservation reservation);
        Task RemoveReservation(Reservation reservation);
        Task<long> GetAmountOfReservations(string title);
        Reservation GetReservationById(int id);
        List<Reservation> GetReservationsByTitle(string title);
        Task<long> GetCountOfReservations();
        long RemoveReservationsByTitle(string movieTitle);
        Task<bool> CheckReservation(Reservation reservation);
        Task UpdateReservation<T>(Reservation reservation);
    }
}
