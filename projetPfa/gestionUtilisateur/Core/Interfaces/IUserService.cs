using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.Core.Models;

namespace gestionUtilisateur.Core.Interfaces
{
    public interface IUserService
    {
        Task<ReturnAddedUserManualy> AddUserManualyAsync(User _user);
    }
}
