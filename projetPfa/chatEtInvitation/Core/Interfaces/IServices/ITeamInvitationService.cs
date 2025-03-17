using chatEtInvitation.API.DTOs;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface ITeamInvitationService
    {
        Task<string> SendInvitationAsync(TeamInvitationDTO invitationDto);
        Task AccepteInvitationAsync(int invId);
        Task<List<MemberTeamInvitationDTOO>> GetUserTeamInvitationsAsync(int userId);
        Task<UserTeamInvitationsResponseDto> GetUserTeamInvitationsNbrAsync(int userId, int adminId);


    }
}
