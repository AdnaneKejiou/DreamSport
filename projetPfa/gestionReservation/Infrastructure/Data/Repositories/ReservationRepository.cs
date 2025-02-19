using gestionReservation.Core.Models;
using Microsoft.EntityFrameworkCore;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    // Ajouter une réservation dans la base de données
    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }

    // Récupérer une réservation par son ID
    public async Task<Reservation> GetByIdAsync(int id)
    {
        return await _context.Reservations.Include(r => r.Status).FirstOrDefaultAsync(r => r.Id == id);
    }

    // Vérifier la disponibilité du terrain
    public async Task<bool> IsTerrainAvailableAsync(int terrainId, DateTime dateRes)
    {
        return !await _context.Reservations.AnyAsync(r => r.TerrainId == terrainId && r.DateRes == dateRes);
    }
}
