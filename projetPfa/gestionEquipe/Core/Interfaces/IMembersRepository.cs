using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Interfaces
{
    public interface IMembersRepository
    {
        Task<Members> KickMemberAsync(Members member);
        
        Task<bool> ExistInTeamAsync(Members member);
    }
}
