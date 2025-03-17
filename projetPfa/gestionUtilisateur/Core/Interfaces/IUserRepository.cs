using gestionUtilisateur.Core.Models;

namespace gestionUtilisateur.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> AddUserManualyAsync(User _user);
        Task<bool> DoesUserWithPhoneExist(string phone,int id);
        Task<bool> DoesUserWithEmailExist(string email,int id);
        Task<bool> DoesUserWithUsernameExist(string username, int id);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetByEmailAsync(string email, int id);
        Task<List<User>> SearchUsersAsync(string searchTerm);


    }
}
