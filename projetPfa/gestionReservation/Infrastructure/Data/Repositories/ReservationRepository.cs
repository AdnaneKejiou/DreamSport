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

    public async Task<int> GetReservationsCountByTerrainAndDateAsync(int terrainId, DateTime dateRes)
    {
        var count = await _context.Reservations
            .Include(r => r.Status) // Join with Status
            .Where(r => r.TerrainId == terrainId
                        && r.DateRes == dateRes
                        && r.Status.Libelle != "annule") // Filter by Status Libelle
            .CountAsync();

        return count;
    }


    public async Task<Reservation?> UpdateReservationAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        var changes = await _context.SaveChangesAsync();

        return changes > 0 ? reservation : null;
    }

}
