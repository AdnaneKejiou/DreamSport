using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IAdminService
    {
        Task<int> LoginAdminAsync(AdminLoginDto userLogin);
    }
}
