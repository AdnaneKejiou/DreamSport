using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IAdminRepository
    {
        bool IsTenantValid(int tenantId);
        Admin? GetAdminByTenantId(int tenantId);
    }
}