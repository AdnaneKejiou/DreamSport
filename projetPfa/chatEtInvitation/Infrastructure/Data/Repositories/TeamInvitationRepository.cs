using chatEtInvitation.Core.Interfaces.IRepositories;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class TeamInvitationRepository : ITeamInvitationRepository
    {
        private readonly AppDbContext _context;

        public TeamInvitationRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
