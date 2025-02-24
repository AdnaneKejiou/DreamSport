using gestionEmployer.Core.Interfaces;
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

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("validate/{AdminId}")]
        public IActionResult ValidateTenant(int AdminId)
        {
            if (!_adminService.ValidateTenant(AdminId))
            {
                return NotFound(); // 404 si le Tenant-ID n'existe pas
            }

            return Ok(); // 200 OK si le Tenant-ID est valide
        }
    }
}
