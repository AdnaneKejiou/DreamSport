using chatEtInvitation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {

        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;
        }

        [HttpPost("block/{userIdToBlock}/{currentUserId}/{AdminId}")]
        public async Task<IActionResult> BlockUser(int userIdToBlock,int currentUserId, int AdminId)
        {
            try
            {
                await _blockService.BlockUserAsync(currentUserId, userIdToBlock , AdminId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors du blocage");
            }
        }

        [HttpPost("unblock/{userIdToUnblock}/{currentUserId}/{AdminId}")]
        public async Task<IActionResult> UnblockUser(int userIdToUnblock,int currentUserId, int AdminId)
        {
            try
            {
                await _blockService.UnblockUserAsync(currentUserId, userIdToUnblock , AdminId);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors du déblocage");
            }
        }

        [HttpGet("blocked/{currentUserId}/{AdmineId}")]
        public async Task<ActionResult<List<int>>> GetBlockedUsers(int currentUserId, int AdminId)
        {
            try
            {
                var blockedUsers = await _blockService.GetBlockedUsersAsync(currentUserId);
                return Ok(blockedUsers);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors de la récupération");
            }
        }

        [HttpGet("is-blocked/{targetUserId}/{currentUserId}/{AdminId}")]
        public async Task<ActionResult<bool>> IsUserBlocked(int targetUserId,int currentUserId,int AdminId)
        {
            try
            {
                var isBlocked = await _blockService.IsUserBlockedAsync(currentUserId, targetUserId);
                return Ok(isBlocked);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors de la vérification");
            }
        }

    }
}
