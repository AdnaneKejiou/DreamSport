using chatEtInvitation.Core.Interfaces.IRepositories;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class ChatAmisMessageRepository : IChatAmisMessageRepository
    {
        private readonly AppDbContext _context;

        public ChatAmisMessageRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
