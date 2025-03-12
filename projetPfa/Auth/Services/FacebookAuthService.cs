using Auth.Dtos;
using Auth.Interfaces;
using Facebook;
using Microsoft.Extensions.Options;

namespace Auth.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly string _facebookAppId;
        private readonly string _facebookAppSecret;

        // Inject your Facebook App credentials from the configuration
        public FacebookAuthService(IConfiguration configuration)
        {
            _facebookAppId = configuration["FacebookAuth:AppId"];
            _facebookAppSecret = configuration["FacebookAuth:AppSecret"];
        }

        public async Task<FacebookUserInfo> ValidateFacebookTokenAsync(string facebookToken)
        {
            try
            {
                var fbClient = new FacebookClient(facebookToken);

                // Make the request to get the user info
                dynamic user = await fbClient.GetTaskAsync("me?fields=id,name,email,picture");

                return new FacebookUserInfo
                {
                    FacebookId = user.id,
                    Email = user.email,
                    Name = user.name,
                    Picture = user.picture.data.url
                };
            }
            catch (Exception)
            {
                // Token validation failed
                return null;
            }
        }
    }
}
