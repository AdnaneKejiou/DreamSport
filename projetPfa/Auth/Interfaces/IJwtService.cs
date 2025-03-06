using System.Security.Claims;

namespace Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(int userId, string role, int AdminId);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
