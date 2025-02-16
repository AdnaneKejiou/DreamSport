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

        public async Task<bool> IsCaptainAsync(int CaptainID, int EquipeId)
        {
            return await _context.Equipes.AnyAsync(e => e.CaptainId == CaptainID && e.Id == EquipeId);
        }
        
        public async Task<bool> ExistWithIdAsync(int id)
        {
            return await _context.Equipes.AnyAsync(e => e.Id == id );
        }

        //supprimer equipe with id (membres inclus grace oncascade)

        // Méthode pour supprimer l'équipe et ses membres
        public async Task SupprimerEquipeAvecMembresAsync(int equipeId)
        {
            var equipe = await _context.Equipes
                .Include(e => e.Members)  // Inclure les membres pour supprimer en cascade
                .FirstOrDefaultAsync(e => e.Id == equipeId);

            if (equipe == null)
            {
                throw new Exception("Équipe non trouvée");
            }

            // Supprimer l'équipe (les membres seront supprimés automatiquement grâce à la cascade)
            _context.Equipes.Remove(equipe);
            await _context.SaveChangesAsync();
        }

        // Méthode pour obtenir une équipe par son ID
        public async Task<Equipe> GetEquipeByIdAsync(int equipeId)
        {
            return await _context.Equipes
                .Include(e => e.Members)  // Inclure les membres
                .FirstOrDefaultAsync(e => e.Id == equipeId);
        }
    }

}
