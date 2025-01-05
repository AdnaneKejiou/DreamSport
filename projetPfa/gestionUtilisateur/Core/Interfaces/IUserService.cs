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

    }
}
