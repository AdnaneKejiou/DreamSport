using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IFacebookAuthService
    {
        Task<FacebookUserInfo> ValidateFacebookTokenAsync(string facebookToken);
    }
}
