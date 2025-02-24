using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;

namespace gestionEmployer.Infrastructure.Data.Repositories
{
    public class AdminRepository:IAdminRepository
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
    }
}
