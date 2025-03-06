using chatEtInvitation.API.DTOs;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface ITeamInvitationService
    {
        Task<string> SendInvitationAsync(TeamInvitationDTO invitationDto);
        Task AccepteInvitationAsync(int invId);
    }
}
