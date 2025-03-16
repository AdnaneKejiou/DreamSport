using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IUserService
    {
        
        Task<GetUserDto?> GetUserByFacebookIdAsync(string facebookId, int AdminId);
        Task<GetUserDto?> AddUserAsync(FacebookUserDto user, int AdminId);
    }
}
