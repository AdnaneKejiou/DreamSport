using gestionEmployer.Core.Interfaces;

namespace gestionEmployer.Core.Services
{
    // PasswordService.cs
    public class PasswordService : IPasswordService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public PasswordService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> VerifyOldPassword(int adminId, int employerId, string oldPassword)
        {
            // Récupération employé
            var employer = await _employeeRepository.GetEmployeeByIdAsync(employerId);

            // Vérifications
            if (employer == null)
                throw new KeyNotFoundException("Employé non trouvé");

            if (employer.AdminId != adminId)
                throw new UnauthorizedAccessException("Action non autorisée");

            // Vérification mot de passe (comparaison directe)
            return employer.Password == oldPassword;
        }

        public async Task ChangePassword(int adminId, int employerId, string newPassword)
        {
            // Vérification que l'admin a le droit (réutilise la même logique)
            await VerifyOldPassword(adminId, employerId, "dummy");
            // Le mot de passe dummy ne sera pas vérifié mais les autres checks le seront

            // Mise à jour du mot de passe
            var employer = await _employeeRepository.GetEmployeeByIdAsync(employerId);
            employer.Password = newPassword;

            await _employeeRepository.UpdateEmployeeAsync(employer);
        }
    }
}
