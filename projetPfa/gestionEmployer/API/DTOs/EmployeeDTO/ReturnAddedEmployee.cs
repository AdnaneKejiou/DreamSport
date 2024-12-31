using gestionEmployer.Core.Models;

namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    public class ReturnAddedEmployee : Employer
    {
        public List<string> errors {  get; set; }
    }
}
