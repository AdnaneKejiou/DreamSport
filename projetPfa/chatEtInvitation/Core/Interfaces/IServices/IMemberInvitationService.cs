using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IMemberInvitationService
    {
        Task SendMemberInvitationAsync(MemberInvitation invitation);
    }
}
