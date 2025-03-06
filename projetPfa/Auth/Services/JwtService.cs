using Auth.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Auth.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly int _tokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;  // Added for refresh token expiration time
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Secret"];
            _tokenExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
            _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"] ?? "30");  // Refresh token expiration time
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
        }

        // Generate Access Token (Short-lived)
        public string GenerateAccessToken(int userId, string role, int AdminId)
        {
            var claims = GetClaims(userId, role, AdminId);
            var key = GetSigningKey();
            var tokenDescriptor = GetTokenDescriptor(claims, key);

            return CreateToken(tokenDescriptor);
        }

        // Generate Refresh Token (Long-lived)
        public string GenerateRefreshToken()
        {
            // Generate a random byte array and convert it to a base64 string for security
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        // Validate Refresh Token
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false, // Don't validate lifetime for refresh tokens
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out var validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        // Generate the claims for the token
        private Claim[] GetClaims(int userId, string role, int AdminId)
        {
            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),  // User ID
                new Claim(ClaimTypes.Role, role),                         // User's Role (e.g., "Admin", "User")
                new Claim("AdminId", AdminId.ToString())                  // Custom claim for AdminId
            };
        }

        // Create a signing key from the secret key
        private SymmetricSecurityKey GetSigningKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }

        // Get token descriptor
        private SecurityTokenDescriptor GetTokenDescriptor(Claim[] claims, SymmetricSecurityKey key)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),  // Access token expiration time
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
        }

        // Create token
        private string CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
