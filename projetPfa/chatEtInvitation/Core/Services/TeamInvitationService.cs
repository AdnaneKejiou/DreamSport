using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.API.DTOs;
using System.Net.Http;
using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Models;
using chatEtInvitation.Infrastructure.Data.Repositories;

namespace chatEtInvitation.Core.Services
{
    public class TeamInvitationService : ITeamInvitationService
    {
        private readonly ITeamInvitationRepository _TeamInvitationRepository;
        private readonly ITeamService _TeamService;

        public TeamInvitationService(ITeamInvitationRepository teamInvitationRepository, ITeamService teamService)
        {
            _TeamInvitationRepository = teamInvitationRepository;
            _TeamService = teamService;
        }

        public async Task AccepteInvitationAsync(int invId)
        {
            TeamInvitation invitation = await _TeamInvitationRepository.GetInvitationByIdAsync(invId);
            if(invitation == null)
            {
                throw new KeyNotFoundException("The invitation not found");
            }

            await _TeamInvitationRepository.DeleteInvitationAsync(invitation);  
            //message broker appele de equipe service pour ajouter le membre
        }

        public async Task<string> SendInvitationAsync(TeamInvitationDTO invitationDto)
        {
            // Vérifier si l'invitation existe déjà
            var existingInvitation = await _TeamInvitationRepository.GetExistingInvitationAsync(invitationDto.Emetteur, invitationDto.Recepteur);
            if (existingInvitation != null)
            {
                return "CONFLICT";
            }

            // Vérifier si l'utilisateur est déjà dans l'équipe
            List<int> MembersIds = await _TeamService.FetchMembersAsync(invitationDto.Emetteur);
            if (MembersIds == null )
            {
                return "TEAM_NOT_FOUND";
            }
            if (MembersIds.Contains(invitationDto.Recepteur))
            {
                return "ALREADY_MEMBER";
            }
           
            // Ajouter l'invitation
            var invitation = TeamInvMapper.SendInvToModel(invitationDto);
            await _TeamInvitationRepository.AddInvitationAsync(invitation);

            return "SUCCESS";
        }

              //---------------------------------

        public async Task<List<MemberTeamInvitationDTOO>> GetUserTeamInvitationsAsync(int userId)
        {
            var invitations = await _TeamInvitationRepository.GetUserTeamInvitationsAsync(userId);

            return invitations.Select(inv => new MemberTeamInvitationDTOO
            {
                Id = inv.Id,
                Emetteur = inv.Emetteur,
                Recerpteur = inv.Recerpteur,
                AdminId = inv.AdminId,
            }).ToList();
        }




        // Méthode pour récupérer les invitations team et le nombre total
        public async Task<UserTeamInvitationsResponseDto> GetUserTeamInvitationsNbrAsync(int userId, int adminId)
        {
            // Récupérer les invitations
            var invitations = await _TeamInvitationRepository.GetUserTeamInvitationsAsync(userId);

            // Récupérer le nombre total d'invitations
            var totalData = await _TeamInvitationRepository.GetUserTeamInvitationsCountAsync(userId, adminId);

            // Mapper les entités vers les DTOs
            var invitationDtos = invitations.Select(inv => new MemberTeamInvitationDTOO
            {
                Id= inv.Id,
                Emetteur = inv.Emetteur,
                Recerpteur = inv.Recerpteur,
                AdminId = inv.AdminId,
            }).ToList();

            // Retourner la réponseMemberTeamInvitationDTOO
            return new UserTeamInvitationsResponseDto
            {
                Invitations = invitationDtos,
                TotalData = totalData
            };
        }
    }
}
