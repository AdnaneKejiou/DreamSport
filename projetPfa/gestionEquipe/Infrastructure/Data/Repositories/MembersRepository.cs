using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionEquipe.Infrastructure.Data.Repositories
{
    public class MembersRepository : IMembersRepository

    {
        private readonly AppDbContext _context;

        public MembersRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<Members> KickMemberAsync(Members member)
        {
            _context.Memberss.Remove(member);

            // Save changes to ensure the deletion is committed
            await _context.SaveChangesAsync();

            // Return the removed member object as confirmation
            return member;
        }

        public async Task<bool> ExistInTeamAsync(Members member)
        {
            return await _context.Memberss.AnyAsync(m => m.UserId == member.UserId && m.EquipeId == member.EquipeId);
            
        }

    }
}
