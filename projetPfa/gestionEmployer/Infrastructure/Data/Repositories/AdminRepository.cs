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

        public async Task<Admin> GetAdminAsync(int id)
        {
            return await _context.Admins
                                 .FirstOrDefaultAsync(admin => admin.Id == id);
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _context.Admins
                                 .ToListAsync();
        }
    }
}
