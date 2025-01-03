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
        private readonly AppDbContext _context;

        public UserController(IUserService userService,AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost]
        [ValidateModelAttribute]
        public async Task<ActionResult<ReturnAddedUserManualy>> AddUserManualyAsync([FromBody] AddUserManualyDTO _userDto)
        {
            var user = UserMapper.AddUserToUser(_userDto);
            var result = await _userService.AddUserManualyAsync(user);
            if (result.errors.Count == 0)
            {
                return Created("/api/users/" + result.IdAdmin, result);

            }
            return BadRequest(result);
        }

        [HttpGet("api/users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Fetch all users from the database
            var users = await _context.Users.ToListAsync();
            
            // Return the list of users
            return Ok(users);
        }
    }
}
