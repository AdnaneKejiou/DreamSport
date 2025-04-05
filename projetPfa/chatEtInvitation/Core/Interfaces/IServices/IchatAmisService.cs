using chatEtInvitation.API.DTOs;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IchatAmisService
    {
        Task<List<AmisChatReturnedDTO>> GetAmisChatInfoAsync(int userId);
        Task<AmisMessageDTO> SendAmisMessageAsync(SendAmisMessageDTO messageDto);
        Task<List<AmisMessageDTO>> GetAmisConversationAsync(int chatAmisId, int adminId);

    }
}
