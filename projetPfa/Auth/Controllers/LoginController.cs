using Auth.Dtos;
using Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IAdminService _adminService;
        private readonly IEmployerService _employerService;

        public LoginController(IUserService userService, IJwtService jwtService, IAdminService adminService, IEmployerService employerService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _adminService = adminService;
            _employerService = employerService;
        }

        // User Login - Issue Access Token and Refresh Token
        [HttpPost("User")]
        public async Task<IActionResult> UserLogin([FromBody] UserLogin model)
        {
            try
            {
                int id = await _userService.LoginUserAsync(model);
                if (id == -1)
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }

                // Generate both the access token and refresh token
                string token = _jwtService.GenerateAccessToken(id, "User", model.AdminId);
                string refreshToken = _jwtService.GenerateRefreshToken();

                // Store refresh token in HttpOnly cookie for security
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30)  // Set expiration time for refresh token (e.g., 30 days)
                });

                return Ok(new { token, refreshToken });
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        //admin
        [HttpPost("Admin")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto model)
        {
            try
            {
                int id = await _adminService.LoginAdminAsync(model);
                if (id == -1)
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }

                // Generate both the access token and refresh token
                string token = _jwtService.GenerateAccessToken(id, "Admin", model.AdminId);
                string refreshToken = _jwtService.GenerateRefreshToken();

                // Store refresh token in HttpOnly cookie for security
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30)  // Set expiration time for refresh token (e.g., 30 days)
                });

                return Ok(new { token, refreshToken });
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        //Employer login
        [HttpPost("Employer")]
        public async Task<IActionResult> EmployerLogin([FromBody] EmployerLoginDto model)
        {
            try
            {
                int id = await _employerService.LoginEmployerAsync(model);
                if (id == -1)
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }

                // Generate both the access token and refresh token
                string token = _jwtService.GenerateAccessToken(id, "Employer", model.AdminId);
                string refreshToken = _jwtService.GenerateRefreshToken();

                // Store refresh token in HttpOnly cookie for security
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30)  // Set expiration time for refresh token (e.g., 30 days)
                });

                return Ok(new { token, refreshToken });
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        // Refresh Token - Issue new Access Token using the Refresh Token
        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken()
        {
            // Retrieve the refresh token from the HttpOnly cookie
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token provided");
            }

            // Validate and get claims from the expired refresh token
            var principal = _jwtService.GetPrincipalFromExpiredToken(refreshToken);

            if (principal == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = principal.FindFirstValue(ClaimTypes.Role);
            var adminId = int.Parse(principal.FindFirstValue("AdminId"));

            // Generate a new access token
            string newToken = _jwtService.GenerateAccessToken(userId, role, adminId);

            return Ok(new { token = newToken });
        }
    }
}
