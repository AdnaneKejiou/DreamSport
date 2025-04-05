using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface ITeamMessageRepository
    {

        Task<List<TeamChatMessage>> GetTeamConversationAsync(int Id);
        Task<TeamChat> ExisteChatTeamAsync(int teamchatId);
        Task<TeamChatMessage> GetLastTeamMessageAsync(int teamChatId);
        Task<string> GetUserMessageStatutAsync(int messageId, int userId);
        Task<int> GetUnreadMessagesCountAsync(int teamChatId, int userId);
        Task<List<TeamChatMessage>> GetTeamConversationWithStatutsAsync(int teamChatId);
        Task<TeamChatMessage> CreateTeamMessageAsync(TeamChatMessage message);
        Task<Statut> GetDefaultMessageStatutAsync();
        Task AddMessageStatutAsync(MessageStatut statut);
        Task UpdateMessageStatutAsync(int messageId, int userId, int newStatutId);

    }
}
