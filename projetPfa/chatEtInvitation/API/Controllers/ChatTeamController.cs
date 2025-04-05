using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatTeamController : ControllerBase
    {
        private readonly IchatTeamService _teamChatService;

        public ChatTeamController(IchatTeamService teamChatService)
        {
            _teamChatService = teamChatService;
        }

        [HttpGet("{teamId}/members/{memberId}")]
        public async Task<ActionResult<TeamChatReturnedDTO>> GetTeamChatInfo(int teamId, int memberId)
        {
            try
            {
                var result = await _teamChatService.GetTeamChatByIdAsync(teamId, memberId);

                if (result == null)
                {
                    return NotFound($"Aucun chat trouvé pour l'équipe {teamId}");
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                // Loguer l'erreur (ex: _logger.LogError(ex, "Erreur lors de la récupération du chat"))
                return StatusCode(500, "Une erreur est survenue lors du traitement de votre demande");
            }
        }

        // Dans chatEtInvitation.API.Controllers.ChatTeamController
        [HttpGet("{teamId}/conversation/{adminId}")]
        public async Task<ActionResult<List<TeamMessageDTO>>> GetFullConversation(
            int teamId, int adminId)
        {
            try
            {
                var result = await _teamChatService.GetFullTeamConversationAsync(teamId, adminId);

                if (result == null)
                {
                    return NotFound($"Aucun chat trouvé pour l'équipe {teamId}");
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Une erreur est survenue lors du traitement de votre demande");
            }
        }

        [HttpGet("{MessageId}/seen/{UserId}/{AdminId}")]

        public async Task MarkMessageAsSeenAsync(int MessageId, int UserId,int AdminId)
        {
            await _teamChatService.MarkMessageAsSeenAsync(MessageId, UserId);


        }



        [HttpPost("send")]
        public async Task<ActionResult<TeamMessageDTO>> SendMessage(
    [FromBody] SendTeamMessageDTO messageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _teamChatService.SendTeamMessageAsync(messageDto, messageDto.AdminId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Une erreur est survenue lors de l'envoi du message");
            }
        }


    }
}
