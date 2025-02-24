using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminAsync(int  id);
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
    }
}
