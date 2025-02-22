using gestionReservation.Core.Models;

public interface IReservationRepository
{
    // Ajouter une nouvelle réservation dans la base de données
    Task AddAsync(Reservation reservation);

    // Récupérer une réservation par son identifiant
    Task<Reservation> GetByIdAsync(int id);

    Task<int> GetReservationsCountByTerrainAndDateAsync(int terrainId, DateTime dateRes);
    Task<List<Reservation>> GetReservationsAsync(DateTime? startDate, DateTime? endDate);

}
