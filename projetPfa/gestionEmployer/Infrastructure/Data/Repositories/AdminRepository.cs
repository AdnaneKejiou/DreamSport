using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionEmployer.Infrastructure.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool IsTenantValid(int tenantId)
        {
            return _context.Admins.Any(t => t.Id == tenantId);
        }

        public Admin? GetAdminByTenantId(int tenantId)
        {
            return _context.Admins.FirstOrDefault(a => a.Id == tenantId);
        }
        public Admin? AddAdmin(Admin admin)
        {
            // Ajouter l'administrateur à la base de données
            _context.Admins.Add(admin);

            // Sauvegarder les changements dans la base de données
            _context.SaveChanges();

            // Retourner l'objet Admin ajouté
            return admin;
        }
        public bool AdminExists(string nom, string login, string phoneNumber)
        {
            // Vérifier si un administrateur existe déjà avec le même nom, login ou numéro de téléphone
            return _context.Admins.Any(a => a.Nom == nom || a.Login == login || a.PhoneNumber == phoneNumber);
        }

        public async Task<Admin> GetByLoginAsync(string login)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Login == login);
        }
    }
}