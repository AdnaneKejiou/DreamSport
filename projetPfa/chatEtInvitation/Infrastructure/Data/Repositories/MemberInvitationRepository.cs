using chatEtInvitation.Core.Interfaces.IRepositories;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class MemberInvitationRepository : IMemberInvitationRepository
    {
        private readonly AppDbContext _context;

        public MemberInvitationRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
