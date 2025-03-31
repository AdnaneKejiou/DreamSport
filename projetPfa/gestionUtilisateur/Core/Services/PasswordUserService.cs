using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Models;
using gestionUtilisateur.Infrastructure.Data.Repositories;

namespace gestionUtilisateur.Core.Services
{
    public class PasswordUserService : IPasswordUserService
    {
        private readonly IUserRepository _userRepository;

        public PasswordUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> VerifyOldUserPassword(int IdAdmin, int userId, string oldPassword)
        {
            // Récupération employé
            var User = await _userRepository.GetByIdAsync(userId);

            // Vérifications
            if (User == null)
                throw new KeyNotFoundException("Employé non trouvé");

            if (User.IdAdmin != IdAdmin)
                throw new UnauthorizedAccessException("Action non autorisée");

            // Vérification mot de passe (comparaison directe)
            return User.Password == oldPassword;
        }

        public async Task ChangeUserPassword(int IdAdmin, int userId, string newPassword)
        {
            // Vérification que l'admin a le droit (réutilise la même logique)
            await VerifyOldUserPassword(IdAdmin, userId, "dummy");
            // Le mot de passe dummy ne sera pas vérifié mais les autres checks le seront

            // Mise à jour du mot de passe
            var User = await _userRepository.GetByIdAsync(userId);
            User.Password = newPassword;

            await _userRepository.UpdateAsync(User);
        }
    }
}
