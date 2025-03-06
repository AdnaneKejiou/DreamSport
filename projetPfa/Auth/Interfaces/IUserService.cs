using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IUserService
    {
        Task<int> LoginUserAsync(UserLogin userLogin);
    }
}
