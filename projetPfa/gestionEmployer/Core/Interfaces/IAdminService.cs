using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IAdminService
    {

        bool ValidateTenant(int tenantId);
        Admin? GetAdmin(int tenantId);

    }
}
