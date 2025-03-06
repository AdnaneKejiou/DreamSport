using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using System;
using System.Threading.Tasks;

namespace chatEtInvitation.Core.Services
{
    public class AmisChatService : IChatService
    {
        private readonly IChatAmisMessageRepository _chatRepository;
        private readonly IMemberInvitationRepository _invitationRepository; // Repository pour gérer les invitations
        private readonly ChatMapper _chatMapper;

        // Injection des repositories et du mapper
        public AmisChatService(
            IChatAmisMessageRepository chatRepository,
            IMemberInvitationRepository invitationRepository,
            ChatMapper chatMapper)
        {
            _chatRepository = chatRepository;
            _invitationRepository = invitationRepository;
            _chatMapper = chatMapper;
        }

     
    }
}
