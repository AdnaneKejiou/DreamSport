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

        private readonly IMemberInvitationService _memberInvitationService;

        public InvitationMemberController(IMemberInvitationService memberInvitationService)
        {
            _memberInvitationService = memberInvitationService;
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

    }
}
