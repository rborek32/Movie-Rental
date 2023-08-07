using ReservationService.Models;

namespace ReservationService.Repositories
{
    public interface IReservationRepository
    {
        Task AddReservation<T>(Reservation reservation);
        Task RemoveReservation(Reservation reservation);
        Task GetAmountOfReservations(string Title);
        Reservation GetReservationById(int Id);
    }
}
