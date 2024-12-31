using gestionEmployer.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace gestionEmployer.Core.Interfaces

{
    public interface IEmployeeRepository
    {
        bool Exists(Expression<Func<Employer, bool>> predicate);
        Task<Employer> GetEmployeeByIdAsync(int id);
        //fontion pour récuperer tous les employe by idAdmin
        Task<IEnumerable<Employer>> GetEmployesByAdminIdAsync(int idAdmin);

        Task<Employer> AddEmployeeAsync(Employer employee);
        Task<Employer?> UpdateEmployeeAsync(Employer employee);

        Task<Employer> DeleteEmployeeAsync(int id);
    }
}
