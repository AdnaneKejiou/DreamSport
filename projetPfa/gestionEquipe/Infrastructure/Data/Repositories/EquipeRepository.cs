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
                var result = await _context.Equipes.AddAsync(_equipe);
                await _context.SaveChangesAsync();

                return result.Entity;
            
        }

        public async Task<int> CountEquipesBySportAndUser(int userId, int SportId)
        {
            return await _context.Equipes
                .Where(e => e.SportId == SportId && e.Members.Any(m => m.UserId == userId))
                .CountAsync();
        }

        public async Task<bool> ExistWithName(string name, int AdminID)
        {
            return await _context.Equipes.AnyAsync(e => e.Name == name && e.AdminId == AdminID);

        }

        public async Task<bool> IsCaptainAsync(int CaptainID, int EquipeId)
        {
            return await _context.Equipes.AnyAsync(e => e.CaptainId == CaptainID && e.Id == EquipeId);
        }
        
        public async Task<Equipe> ExistWithIdAsync(int id)
        {
            return await _context.Equipes
                                     .FirstOrDefaultAsync(m => m.Id == id);
        }


    }

}
