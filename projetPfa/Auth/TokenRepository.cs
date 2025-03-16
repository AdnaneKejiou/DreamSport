using Auth.Interfaces;
using Auth.Model;
using Microsoft.EntityFrameworkCore;

namespace Auth
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext context;

        public TokenRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ValidateToken> GetValidateTokenAsync(string token)
        {
            return await context.validateTokens.FirstOrDefaultAsync(v => v.Token == token);
        }

        public async Task<ValidateToken> AddTokenAsync(ValidateToken token)
        {
            await context.validateTokens.AddAsync(token);
            await context.SaveChangesAsync(); 

            return token;
        }

        public async Task<ValidateToken> GetFacebookTokenAsync(string id)
        {
            return await context.validateTokens.FirstOrDefaultAsync(v => v.facebookId == id);
        }
    }
}
