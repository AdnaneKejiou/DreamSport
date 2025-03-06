using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IEmployerService
    {
        Task<int> LoginEmployerAsync(EmployerLoginDto userLogin);
    }
}
