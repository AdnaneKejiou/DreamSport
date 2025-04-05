using chatEtInvitation.API.DTOs;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IchatTeamService
    {
        Task<TeamChatReturnedDTO> GetTeamChatByIdAsync(int idEquipe, int idMember);
        Task<List<TeamMessageDTO>> GetFullTeamConversationAsync(int teamId, int adminId);
        Task<TeamMessageDTO> SendTeamMessageAsync(SendTeamMessageDTO messageDto, int adminId);
        Task MarkMessageAsSeenAsync(int messageId, int userId);
        Task CreateTeamChat(int teamId, int adminId);

    }
}
