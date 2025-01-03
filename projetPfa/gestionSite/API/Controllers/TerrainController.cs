using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.API.Mappers;
using gestionSite.Core.Interfaces.TerrainInterfaces;
using gestionSite.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gestionSite.API.Filters;
using gestionSite.Infrastructure.Mappers;

namespace gestionSite.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class TerrainController : ControllerBase
    {
        private readonly ITerrainService _terrainService;

        public TerrainController(ITerrainService terrainService)
        {
            _terrainService = terrainService;
        }

        // Consulter tous les terrains d'un administrateur
        [HttpGet("{idAdmin}")]
        public async Task<ActionResult<IEnumerable<Terrain>>> GetTerrainsByAdminAsync(int idAdmin)
        {
            if (idAdmin <= 0)
            {
                return BadRequest("Invalid admin ID. It must be greater than 0.");
            }

            var terrains = await _terrainService.GetAllTerrainsByAdminAsync(idAdmin);

            if (terrains == null || !terrains.Any())
            {
                return NotFound($"No terrains found for admin with ID {idAdmin}.");
            }

            return Ok(terrains);
        }

        // Ajouter un terrain
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<ActionResult<Terrain>> AddTerrainAsync([FromBody] AddTerrainDto addTerrain)
        {
            if (addTerrain == null)
            {
                return BadRequest("Invalid terrain data.");
            }

            var terrain = TerrainMapper.AddTerrainDtoToTerrain(addTerrain);
            var result = await _terrainService.AddTerrainAsync(terrain);

            if (result == null)
            {
                return BadRequest("An error occurred while adding the terrain.");
            }

            return Created("/api/Terrain/" + result.IdAdmin, result);
            
        }

        // Modifier un terrain
        [HttpPut]
        [ValidateModelAttribute]
        public async Task<ActionResult<Terrain>> UpdateTerrainAsync([FromBody] UpdateTerrainDto updateTerrain)
        {
            if (updateTerrain == null || updateTerrain.Id <= 0)
            {
                return BadRequest("Invalid terrain data.");
            }

            var terrain = TerrainMapper.UpdateTerrainDtoToTerrain(updateTerrain);
            var result = await _terrainService.UpdateTerrainAsync(terrain);

            if (result == null)
            {
                return BadRequest("Failed to update terrain.");
            }

            return Ok(result);
        }

        // Supprimer un terrain
        [HttpDelete("{id}")]
        public async Task<ActionResult<Terrain>> DeleteTerrainAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("The ID must be greater than 0.");
            }

            var result = await _terrainService.DeleteTerrainAsync(id);

            if (result == null)
            {
                return NotFound("Terrain not found.");
            }

            return Ok(result);
        }
    }
}

