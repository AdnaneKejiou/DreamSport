using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gestionUtilisateur.API.Filters;
using gestionUtilisateur.Core.Models;
using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.API.Mappers;
using gestionUtilisateur.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using gestionUtilisateur.Infrastructure.Data;
namespace gestionUtilisateur.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ValidateModelAttribute]
        public async Task<ActionResult<ReturnAddedUserManualy>> AddUserManualyAsync([FromBody] AddUserManualyDTO _userDto)
        {
            var user = UserMapper.AddUserToUser(_userDto);
            var result = await _userService.AddUserManualyAsync(user);
            if (result.errors.Count == 0)
            {
                return Created("/api/users/" + result.AdminId, result);

            }
            return BadRequest(result);
        }


        [HttpPut("{id}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UpdateUserDto dto)
        {
            if (id <= 0)
                return BadRequest("ID utilisateur invalide.");

            var result = await _userService.UpdateUserAsync(id, dto);
            if (result == null)
                return NotFound($"Utilisateur avec l'ID {id} introuvable.");

            return Ok(new { message = "Profil mis à jour avec succès.", data = result });
        }


        [HttpDelete("{id}/{AdminId}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
        [HttpPut("update-sport-data/{id}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> UpdateSportDataAsync(int id, [FromBody] UpdateSportDataDTO dto)
        {
            if (id <= 0)
                return BadRequest("ID utilisateur invalide.");

            var result = await _userService.UpdateSportDataAsync(id, dto);
            if (!result)
                return NotFound($"Utilisateur avec l'ID {id} introuvable.");

            return Ok(new { message = "Profil sportif mis à jour avec succès." });
        }

        [HttpPut("recover-password")]
        [ValidateModelAttribute]
        public async Task<IActionResult> RecupererPassword([FromBody] RecupererPasswordDTO dto)
        {

            // Appel au service
            var userDto = await _userService.RecupererPasswodAsync(dto);

            if (userDto.error != null)
            {
                return BadRequest(userDto);
            }
            return Ok(userDto);

        }

        [HttpGet("{id}/{AdminId}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            User user = await _userService.GetUserAsync(id);
            if (user == null) { return NotFound(); }
            return Ok(user);
        }
        [HttpPut("ResetConteur/{id}")]
        public async Task<IActionResult> ResetConteurResAnnuler(int id)
        {
            var success = await _userService.ResetConteurResAnnulerAsync(id);
            if (!success)
                return NotFound($"Utilisateur avec l'ID {id} introuvable.");

            return Ok(new { message = "Conteur_res_annuler réinitialisé avec succès." });
        }

        [HttpPut("check-reservation-annule/{id}")]
        public async Task<IActionResult> CheckAndIncrementReservationAnnule(int id)
        {
            try
            {
                var success = await _userService.CheckAndIncrementReservationAnnuleAsync(id);
                if (!success)
                    return Conflict(new { message = "Utilisateur déjà bloqué des réservations." });

                return Ok(new { message = "Le compteur des réservations annulées a été mis à jour." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = "Une erreur interne s'est produite. Veuillez réessayer plus tard." });
            }

        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUser([FromBody] LoginDto model)
        {
            try
            {
                int id = await _userService.Login(model);
                if(id == -3)
                {
                    return Forbid("cant login please contact the support");
                }
                return Ok(id);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error ");
            }
        }

        //--search user

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<List<UserDto>>> SearchUsersAsync(string searchTerm)
        {
            var result = await _userService.SearchUsersAsync(searchTerm);
            return Ok(result);
        }
    }
}
