using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class MemberInvitationRepository : IMemberInvitationRepository
    {
        private readonly AppDbContext _context;

        public MemberInvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MemberInvitation> GetMemberInvitationAsync(int emetteur, int recepteur)
        {
            return await _context.MemberInvitations
                .FirstOrDefaultAsync(i => i.Emetteur == emetteur && i.Recerpteur == recepteur);
        }

        public async Task AddInvitationAsync(MemberInvitation invitation)
        {
            _context.MemberInvitations.Add(invitation);
            await _context.SaveChangesAsync();
        }
        // Injection du contexte de la base de données

        // Méthode pour refuser (supprimer) une invitation
        public async Task<bool> RefuserInvitation(int invitationId)
        {
            // Recherche de l'invitation par son ID et l'ID du récepteur (utilisateur qui refuse)
            var invitation = await _context.MemberInvitations
                                            .FirstOrDefaultAsync(i => i.Id == invitationId );

            // Vérifier si l'invitation existe et si l'utilisateur est bien celui qui l'a reçue
            if (invitation == null)
            {
                return false; // L'invitation n'existe pas ou l'utilisateur n'est pas celui qui a reçu cette invitation
            }

            // Si l'invitation est trouvée, on la supprime de la base de données
            _context.MemberInvitations.Remove(invitation);
            await _context.SaveChangesAsync(); // Sauvegarder les changements dans la base de données

            return true; // Invitation refusée et supprimée avec succès
        }




        // Injection du contexte de la base de données

        // Méthode pour refuser (supprimer) une invitation
        public async Task<bool> RefuserInvitation(int invitationId)
        {
            // Recherche de l'invitation par son ID et l'ID du récepteur (utilisateur qui refuse)
            var invitation = await _context.MemberInvitations
                                            .FirstOrDefaultAsync(i => i.Id == invitationId );

            // Vérifier si l'invitation existe et si l'utilisateur est bien celui qui l'a reçue
            if (invitation == null)
            {
                return false; // L'invitation n'existe pas ou l'utilisateur n'est pas celui qui a reçu cette invitation
            }

            // Si l'invitation est trouvée, on la supprime de la base de données
            _context.MemberInvitations.Remove(invitation);
            await _context.SaveChangesAsync(); // Sauvegarder les changements dans la base de données

            return true; // Invitation refusée et supprimée avec succès
        }




    }

}
