using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Exceptions;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using chatEtInvitation.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationMemberController : ControllerBase
    {

        private readonly IMemberInvitationService _memberInvitationService;
        private readonly IInvitationService _invitationService;


        public InvitationMemberController(IMemberInvitationService memberInvitationService,IInvitationService invitationService)
        {
            _memberInvitationService = memberInvitationService;
            _invitationService = invitationService;

        }


        [HttpPost("send")]
        public async Task<IActionResult> SendMemberInvitation([FromBody] AddInvMemberDto dto)
        {
            if(dto.Recerpteur == dto.Emetteur)
            {
                return BadRequest();
            }
            MemberInvitation invitation = MemberInvitationMapper.AddDtoToModel(dto);
            try
            {
                await _memberInvitationService.SendMemberInvitationAsync(invitation);
                return Ok(new { message = "Invitation sent successfully!" });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 Conflict
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(403, new { message = ex.Message }); // 403 Forbidden
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred.", error = ex.Message });
            }
        }

      

        // Endpoint API pour refuser une invitation
        [HttpDelete("Refuser/{id}")]
        public async Task<IActionResult> RefuserInvitation(int id)
        {
            // Appel du service pour refuser l'invitation
            var success = await _invitationService.RefuserInvitation(id);

            if (success)
            {
                return NoContent();
            }

            return StatusCode(500, new { message = "L'invitation n'a pas pu être refusée. Vérifiez l'ID de l'invitation ou l'ID de l'utilisateur." });
        }

       

      
    }
}
