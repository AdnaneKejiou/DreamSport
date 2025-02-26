using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class BloqueListRepository : IBloqueListRepository
    {
        private readonly AppDbContext _context;

        public BloqueListRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BloqueList> IsBlockedAsync(int Bloked, int BlokedBy)
        {
            return await _context.BloqueList
                .FirstOrDefaultAsync(b => b.Bloked == Bloked && b.BlokedBy == BlokedBy);
        }

    }
}
