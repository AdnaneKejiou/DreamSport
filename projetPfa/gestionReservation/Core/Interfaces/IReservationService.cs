using gestionReservation.API.DTOs;

public interface IReservationService
{
    // Créer une réservation après toutes les vérifications
    Task<bool> CreateReservationAsync(ReservationDto reservationDto);
}
