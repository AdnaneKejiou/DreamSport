using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public bool ValidateTenant(int tenantId)
        {
            return _adminRepository.IsTenantValid(tenantId);
        }

        public Admin? GetAdmin(int tenantId)
        {
            return _adminRepository.GetAdminByTenantId(tenantId);
        }
    }
}
