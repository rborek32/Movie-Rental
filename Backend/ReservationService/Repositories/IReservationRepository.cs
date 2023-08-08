using ReservationService.Models;

namespace ReservationService.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservations();
        Task AddReservation<T>(Reservation reservation);
        Task RemoveReservation(Reservation reservation);
        Task<long> GetAmountOfReservations(string Title);
        Reservation GetReservationById(int Id);
        Reservation GetReservationByTitle(string Title);
        Task<long> GetCountOfReservations();
        long RemoveReservationsByTitle(string movieTitle);
    }
}
