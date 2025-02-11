using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionEquipe.Infrastructure.Data.Repositories
{
    public class EquipeRepository : IEquipeRepository
    {
        private readonly AppDbContext _context; 

        public EquipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Equipe> AddEquipeAsync(Equipe _equipe)
        {
            try
            {
                var result = await _context.Equipes.AddAsync(_equipe);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                // Handle exceptions as needed (e.g., log the error)
                return null;
            }
        }

        public async Task<int> CountEquipesBySportAndUser(int userId, int SportId)
        {
            return await _context.Equipes
                .Where(e => e.CaptainId == userId && e.SportId == SportId)
                .CountAsync();
        }

        public async Task<bool> ExistWithName(string name, int AdminID)
        {
            return await _context.Equipes.AnyAsync(e => e.Name == name && e.AdminId == AdminID);

        }

    }

}
