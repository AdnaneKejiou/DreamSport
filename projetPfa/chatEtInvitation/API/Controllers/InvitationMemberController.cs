using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Exceptions;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationMemberController : ControllerBase
    {

        private readonly IMemberInvitationService _memberInvitationService ;
        private readonly IInvitationService _invitationService;


        public InvitationMemberController(IMemberInvitationService memberInvitationService , IInvitationService invitationService)
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
            try
            {
                var success = await _invitationService.RefuserInvitation(id);

                if (success)
                {
                    return NoContent();
                }

            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return BadRequest();
            
        }

        //Get invitation By Id
        [HttpGet("Get/{id}")]

        public async Task<IActionResult> GetInvitationByIdAsync(int id)
        {
            //Appel du service pour get Invitation 
            var invitation = await _invitationService.GetInvitationByIdAsync(id);
            if (invitation == null)
            {
                // si l'invitation ne se trouve pas return 404 not found avec le message 
                return NotFound(new {message= "Invitation non trouvée" });
            }
            return Ok(invitation);
        }

        //Accepter invitation 


        [HttpPost("accepter/{invitationId}")]
        public async Task<IActionResult> AccepterInvitation(int invitationId)
        {
            try
            {
                var success = await _invitationService.AccepterInvitationAsync(invitationId);
                if (!success)
                {
                    return StatusCode(500, "an error happen");
                }
                return Ok("Invitation acceptée et chat créé avec succès.");
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }




    }
}
