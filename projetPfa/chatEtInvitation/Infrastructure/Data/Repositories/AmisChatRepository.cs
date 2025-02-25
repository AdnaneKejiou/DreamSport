using chatEtInvitation.Core.Interfaces.IRepositories;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class AmisChatRepository : IAmisChatRepository
    {
        private readonly AppDbContext _context;

        public AmisChatRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
