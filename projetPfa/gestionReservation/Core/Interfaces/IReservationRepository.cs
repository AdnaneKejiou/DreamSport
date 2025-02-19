using gestionReservation.Core.Models;

public interface IReservationRepository
{
    // Ajouter une nouvelle réservation dans la base de données
    Task AddAsync(Reservation reservation);

    // Récupérer une réservation par son identifiant
    Task<Reservation> GetByIdAsync(int id);

    // Vérifier la disponibilité d'un terrain à une date donnée
    Task<bool> IsTerrainAvailableAsync(int terrainId, DateTime dateRes);
}
