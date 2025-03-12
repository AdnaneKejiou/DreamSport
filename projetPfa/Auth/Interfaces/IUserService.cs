using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IUserService
    {
        Task<GetUserDto?> GetUserByEmailAsync(string email); 
        Task<GetUserDto?> GetUserByFacebookIdAsync(string facebookId);
    }
}
