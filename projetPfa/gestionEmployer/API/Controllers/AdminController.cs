using gestionEmployer.API.DTOs.AdminDTO;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.API.Mappers;
using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionEmployer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IPasswordServiceAdmin _passwordServiceadmin;


        public AdminController(IAdminService adminService, IPasswordServiceAdmin passwordServiceadmin)
        {
            _adminService = adminService;
            _passwordServiceadmin = passwordServiceadmin;
        }

        [HttpGet("validate/{AdminId}")]
        public IActionResult ValidateTenant(int AdminId)
        {
            if (!_adminService.ValidateTenant(AdminId))
            {
                return NotFound(); // 404 si le Tenant-ID n'existe pas
            }

            return Ok(AdminId); // 200 OK si le Tenant-ID est valide
        }

        // Action pour ajouter un administrateur
        [HttpPost("ajouter")]
        public IActionResult AjouterAdmin([FromBody] AjouterAdminDTO ajouterAdminDTO)
        {
            if (ajouterAdminDTO == null)
            {
                return BadRequest("Les données de l'administrateur sont invalides.");
            }

            try
            {
                // Mapper le DTO en Admin
                Admin admin = AdminMapper.AdminDTOToAdmin(ajouterAdminDTO);

                // Appeler la méthode AjouterAdmin du service
                AdminAddedDTO adminAddedDTO = _adminService.AjouterAdmin(admin);

                // Vérifier si des erreurs existent dans le DTO
                if (adminAddedDTO.errors.Any())
                {
                    // Retourner une mauvaise demande (BadRequest) avec les erreurs
                    return BadRequest(adminAddedDTO.errors);
                }

                // Retourner une réponse OK avec les informations du DTO AdminAddedDTO
                return Ok(adminAddedDTO);
            }
            catch (Exception ex)
            {
                // En cas d'erreur générale, retourner une erreur interne avec le message d'exception
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }


        [HttpPost("validate")]
        public async Task<IActionResult> ValidateAdmin([FromBody] AdminLoginDto dto)
        {
            try
            {
                SendLoginEmployeeDto admin = await _adminService.ValidateLoginAsync(dto);
                if (admin == null)
                {
                    return Unauthorized("Login or password are incorrect");
                }
                return Ok(admin);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }

        }

        //change password
        [HttpPut("changeAdminPassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeAdminPasswordDto ChangeAdminPasswordDto)
        {
            try
            {
                // Vérification de l'ancien mot de passe
                var isValid = await _passwordServiceadmin.VerifyOldPasswordAdmin(ChangeAdminPasswordDto.AdminId, ChangeAdminPasswordDto.OldPassword);

                if (!isValid)
                    return BadRequest("Ancien mot de passe incorrect");

                // Changement du mot de passe
                await _passwordServiceadmin.ChangePasswordAdmin(ChangeAdminPasswordDto.AdminId, ChangeAdminPasswordDto.NewPassword);

                return Ok("Mot de passe mis à jour avec succès");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }
    }
}
