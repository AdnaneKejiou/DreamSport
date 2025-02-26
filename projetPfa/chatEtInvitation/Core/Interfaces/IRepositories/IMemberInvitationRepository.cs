using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface IMemberInvitationRepository
    {
        Task<MemberInvitation> GetMemberInvitationAsync(int emetteur, int recepteur);
        Task AddInvitationAsync(MemberInvitation invitation);
        Task<bool> RefuserInvitation(int id  );
        Task<bool> RefuserInvitation(int id  );
    }
}
