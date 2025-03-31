using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IAdminRepository
    {
        bool IsTenantValid(int tenantId);
        Admin? GetAdminByTenantId(int tenantId);
        Admin? AddAdmin(Admin admin);
        bool AdminExists(string nom, string login, string phoneNumber);
        Task<Admin> GetByLoginAsync(string login);
        Task<Admin?> UpdateAdminAsync(Admin admin);


    }
}