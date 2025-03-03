namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IInvitationService
    {
        Task<bool> RefuserInvitation(int id );
    }
}
