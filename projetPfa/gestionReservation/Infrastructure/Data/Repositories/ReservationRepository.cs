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
        try
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the inner exception if it exists
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            throw;  // Rethrow the exception to preserve the stack trace
        }
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
            .Where(r => r.IdTerrain == terrainId
                        && r.DateRes == dateRes
                        && r.Status.Libelle != "annule") // Filter by Status Libelle
            .CountAsync();

        return count;
    }

    public async Task<List<Reservation>> GetReservationsAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Reservations.AsQueryable(); // Supprime le filtre sur le complexe

        // Si aucune date n'est spécifiée, on récupère seulement les réservations futures
        if (!startDate.HasValue && !endDate.HasValue)
        {
            query = query.Where(r => r.DateRes >= DateTime.Now);
        }
        else
        {
            if (startDate.HasValue)
                query = query.Where(r => r.DateRes >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(r => r.DateRes <= endDate.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Reservation?> UpdateReservationAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        var changes = await _context.SaveChangesAsync();

        return changes > 0 ? reservation : null;
    }

}
