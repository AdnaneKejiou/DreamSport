using chatEtInvitation.Core.Interfaces.IRepositories;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class TeamMessageRepository : ITeamMessageRepository
    {
        private readonly AppDbContext _context;

        public TeamMessageRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
