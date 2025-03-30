using gestionEmployer.Core.Interfaces;

namespace gestionEmployer.Core.Services
{
    // PasswordService.cs
    public class PasswordServiceAdmin : IPasswordServiceAdmin
    {
        private readonly IAdminRepository _adminRepository;

        public PasswordServiceAdmin(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<bool> VerifyOldPasswordAdmin(int adminId, string oldPassword)
        {
            // Récupération employé
            var Admin = _adminRepository.GetAdminByTenantId(adminId);

            // Vérifications
            if (Admin == null)
                throw new KeyNotFoundException("Admin non trouvé");


            // Vérification mot de passe (comparaison directe)
            return Admin.Password == oldPassword;
        }

        public async Task ChangePasswordAdmin(int adminId, string newPassword)
        {
            // Vérification que l'admin a le droit (réutilise la même logique)
            await VerifyOldPasswordAdmin(adminId, "dummy");
            // Le mot de passe dummy ne sera pas vérifié mais les autres checks le seront

            // Mise à jour du mot de passe
            var Admin =  _adminRepository.GetAdminByTenantId(adminId);
            Admin.Password = newPassword;

            await _adminRepository.UpdateAdminAsync(Admin);
        }
    }
}
