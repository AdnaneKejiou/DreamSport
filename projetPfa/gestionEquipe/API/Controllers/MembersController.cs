using gestionEquipe.API.DTOs;
using gestionEquipe.API.Mappers;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Services;
using gestionEquipe.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestionEquipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMembersService _membersService;

        public MembersController(IMembersService membersService)
        {
            _membersService = membersService;
        }

        [HttpDelete]
        public async Task<ActionResult<AddedEquipeDTO>> KickMemberAsync([FromBody] DeleteMemberDTO member)
        {
            var _member = MembersMapper.DeleteDTOtoModel(member);
            try
            {
                var deletedMember = await _membersService.KickMemberAsync(_member);
                return Ok(new { message = "Member removed successfully.", _member = deletedMember });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Handle when team or member is not found
            }
            
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }
    }
}
