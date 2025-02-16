using gestionEquipe.API.DTOs;
using gestionEquipe.API.Mappers;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestionEquipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipeController : ControllerBase
    {
        private readonly IEquipeService _equipeService;

        public EquipeController(IEquipeService equipeService)
        {
            _equipeService= equipeService;
        }

        [HttpPost]
        public async Task<ActionResult<AddedEquipeDTO>> AddEquipeAsync([FromBody] AddEquipeDTO addEquipe)
        {
            var equipe = EquipeMapper.AddToModel(addEquipe);
            AddedEquipeDTO result = await _equipeService.AddEquipeAsync(equipe);
            if (result.Errors.Count() > 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
            //return Created("/api/faq/" + result.IdAdmin, result);
        }



        //   supprimer une équipe avec ses membres
        [HttpDelete("{equipeId}")]
        public async Task<IActionResult> SupprimerEquipe(int equipeId)
        {
            try
            {
                await _equipeService.SupprimerEquipeAvecMembresAsync(equipeId);
                return NoContent(); // Réponse 204 No Content lorsque la suppression est réussie
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // En cas d'erreur, retour d'une réponse 400 BadRequest
            }
        }

        
        

    }
}
