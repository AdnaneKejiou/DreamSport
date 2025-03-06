using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chatEtInvitation.API.DTOs;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationTeamController : ControllerBase
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public InvitationTeamController (ITeamInvitationService invitationService)
        {
            _teamInvitationService = invitationService;
        }

        [HttpPost]
        public async Task<IActionResult> SendInvitation([FromBody] TeamInvitationDTO invitationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _teamInvitationService.SendInvitationAsync(invitationDto);

            return result switch
            {
                "CONFLICT" => Conflict(new { message = "Une invitation existe déjà." }),
                "TEAM_NOT_FOUND" => NotFound(new {message = "Team not found"}),
                "ALREADY_MEMBER" => BadRequest(new { message = "L'utilisateur est déjà membre de l'équipe." }),
                "SUCCESS" => Ok(new { message = "Invitation envoyée avec succès." }),
                _ => StatusCode(500, new { message = "Erreur inconnue." })
            };
        }

        [HttpPost("accepter/{invitationId}")]
        public async Task<IActionResult> AccepterInvitation(int invitationId)
        {
            try
            {
                await _teamInvitationService.AccepteInvitationAsync(invitationId);
                return Ok("Invitation acceptée et chat créé avec succès.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error happen");
            }

        }
    }
}
