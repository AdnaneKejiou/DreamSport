using chatEtInvitation.Core.Interfaces.IRepositories;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class TeamChatRepository : ITeamChatRepository
    {
        private readonly AppDbContext _context;

        public TeamChatRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
