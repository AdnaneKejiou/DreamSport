using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace chatEtInvitation.Core.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IMemberInvitationRepository _memberInvitationRepository;

        // Injection du repository dans le service
        public InvitationService(IMemberInvitationRepository memberInvitationRepository)
        {
            _memberInvitationRepository = memberInvitationRepository;
        }

        // Méthode pour refuser (supprimer) une invitation
        public async Task<bool> RefuserInvitation(int Id )
        {
            // Appel de la méthode du repository pour refuser l'invitation en passant les paramètres nécessaires
            return await _memberInvitationRepository.RefuserInvitation(Id);
        }
    }

}
