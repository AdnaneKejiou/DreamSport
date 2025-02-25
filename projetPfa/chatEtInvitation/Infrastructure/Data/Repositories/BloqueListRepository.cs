using chatEtInvitation.Core.Interfaces.IRepositories;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class BloqueListRepository : IBloqueListRepository
    {
        private readonly AppDbContext _context;

        public BloqueListRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
