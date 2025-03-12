using Auth.Dtos;
using Auth.Interfaces;

namespace Auth.Services
{
    public class FacebookLoginAdapter : ILoginService
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserService _userService;

        public FacebookLoginAdapter(IFacebookAuthService facebookAuthService, IUserService userService)
        {
            _facebookAuthService = facebookAuthService;
            _userService = userService;
        }

        public async Task<GetUserDto?> ValidateUserAsync(UserLogin model)
        {
            var facebookUser = await _facebookAuthService.ValidateFacebookTokenAsync(model.FacebookToken);
            if (facebookUser == null)
                throw new UnauthorizedAccessException("Invalid Facebook token.");

            return await _userService.GetUserByEmailAsync(facebookUser.Email);
        }
    }
}
