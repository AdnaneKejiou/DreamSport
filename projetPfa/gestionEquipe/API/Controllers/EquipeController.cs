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

    }
}
