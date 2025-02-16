using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Services
{
    public class MembersService : IMembersService 
    {
        private readonly IMembersRepository membersRepository;
        private readonly IEquipeRepository equipeRepository;

        public MembersService(IMembersRepository membersRepository, IEquipeRepository equipeRepository)
        {
            this.membersRepository = membersRepository;
            this.equipeRepository = equipeRepository;
        }

        public async Task<Members> KickMemberAsync(Members member)
        {
            if (!await equipeRepository.ExistWithIdAsync(member.EquipeId))
            {
                throw new KeyNotFoundException("Team not found.");
            }

            if(!await membersRepository.ExistInTeamAsync(member))
            {
                throw new KeyNotFoundException("Member not found in this team.");
            }

            
            var deletedMember = await membersRepository.KickMemberAsync(member);
            return deletedMember;
        }    
    }
}
