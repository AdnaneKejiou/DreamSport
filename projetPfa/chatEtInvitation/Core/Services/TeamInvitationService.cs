using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;

namespace chatEtInvitation.Core.Services
{
    public class TeamInvitationService : ITeamInvitationService
    {
        private readonly ITeamInvitationRepository _TeamInvitationRepository;

        public TeamInvitationService(ITeamInvitationRepository teamInvitationRepository)
        {
            _TeamInvitationRepository = teamInvitationRepository;
        }

        public async Task AccepteInvitationAsync()
        {

        }
    }
}
