using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employer> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<Employer>> GetEmployesByAdminIdAsync(int idAdmin);

        Task<ReturnAddedEmployee> AddEmployeeAsync(Employer employee);
        Task<Employer?> UpdateEmployeeAsync(Employer employee);
        Task<Employer> DeleteEmployeeAsync(int id);
        Task<Employer> ModifyProfileAsync(Employer employer);
    }
}
