using gestionReservation.API.DTOs;
using gestionReservation.Core.Models;

public interface IReservationService
{
    // Créer une réservation après toutes les vérifications
    Task<AddReservationDto> AjouterReservationAsync(Reservation reservation);
    Task<Reservation> ReservationStatusUpdateAsync(UpdateStatusDTO dto);
    Task<List<ReturnedListReservationsDTO>> GetReservationsAsync( GetReservationsDTO filter);

}
