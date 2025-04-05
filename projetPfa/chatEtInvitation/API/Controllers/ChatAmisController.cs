using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatAmisController : ControllerBase
    {
        private readonly IchatAmisService _chatAmisService;

        public ChatAmisController(IchatAmisService chatAmisService)
        {
            _chatAmisService = chatAmisService;
        }

        [HttpGet("{userId}/{AdminId}")]
        public async Task<ActionResult<List<AmisChatReturnedDTO>>> GetAmisChatInfo(int userId)
        {
            try
            {
                var result = await _chatAmisService.GetAmisChatInfoAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Une erreur est survenue");
            }
        }

        [HttpPost("send")]
        public async Task<ActionResult<AmisMessageDTO>> SendMessage([FromBody] SendAmisMessageDTO messageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _chatAmisService.SendAmisMessageAsync(messageDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Erreur lors de l'envoi du message");
            }
        }

        [HttpGet("{chatAmisId}/conversation/{AdminId}")]
        public async Task<ActionResult<List<AmisMessageDTO>>> GetConversation(int chatAmisId, int adminId)
        {
            try
            {
                var result = await _chatAmisService.GetAmisConversationAsync(chatAmisId, adminId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Erreur lors de la récupération de la conversation");
            }
        }
    }
}
