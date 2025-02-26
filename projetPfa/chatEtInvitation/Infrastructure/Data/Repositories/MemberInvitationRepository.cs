using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class MemberInvitationRepository : IMemberInvitationRepository
    {
        private readonly AppDbContext _context;

        public MemberInvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MemberInvitation> GetMemberInvitationAsync(int emetteur, int recepteur)
        {
            return await _context.MemberInvitations
                .FirstOrDefaultAsync(i => i.Emetteur == emetteur && i.Recerpteur == recepteur);
        }

        public async Task AddInvitationAsync(MemberInvitation invitation)
        {
            _context.MemberInvitations.Add(invitation);
            await _context.SaveChangesAsync();
        }
    }
}
