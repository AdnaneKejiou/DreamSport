using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class TeamInvitationRepository : ITeamInvitationRepository
    {
        private readonly AppDbContext _context;

        public TeamInvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeamInvitation> GetExistingInvitationAsync(int emetteur, int recepteur)
        {
            return await _context.TeamInvitations
                .FirstOrDefaultAsync(i => i.Emetteur == emetteur && i.Recerpteur == recepteur);
        }

        public async Task AddInvitationAsync(TeamInvitation invitation)
        {
            await _context.TeamInvitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task<TeamInvitation> GetInvitationByIdAsync(int invId)
        {
            return await _context.TeamInvitations.FindAsync(invId);
        }

        public async Task DeleteInvitationAsync(TeamInvitation invitation)
        {
            _context.TeamInvitations.Remove(invitation);
            await _context.SaveChangesAsync(); // Sauvegarder les changements dans la base de données
        }

    }
}
