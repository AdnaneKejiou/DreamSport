using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.Core.Models;

namespace gestionUtilisateur.Core.Interfaces
{
    public interface IUserService
    {
        Task<ReturnAddedUserManualy> AddUserManualyAsync(User _user);
        Task<bool> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateSportDataAsync(int userId, UpdateSportDataDTO dto);
        Task<ReturnForgotPasswordDTO> RecupererPasswodAsync( RecupererPasswordDTO dTO);
        Task<User> GetUserAsync(int userId);
        Task<bool> ResetConteurResAnnulerAsync(int userId);
        Task<bool> CheckAndIncrementReservationAnnuleAsync(int userId);
        Task<int> Login(LoginDto login);
        Task<List<UserDto>> SearchUsersAsync(string searchTerm);



    }
}
