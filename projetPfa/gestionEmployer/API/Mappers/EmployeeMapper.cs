using gestionEmployer.API.DTOs.DTOs;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.Core.Models;

namespace gestionEmployer.API.Mappers
{
    public class EmployeeMapper
    {
        public static Employer AddEmployeeDTOToEmployer(AddEmployeeDTO _AddEmployeeDTO)
        {
            return new Employer
            {
                IdAdmin = _AddEmployeeDTO.IdAdmin,
                Nom = _AddEmployeeDTO.Nom,
                Prenom = _AddEmployeeDTO.Prenom,
                Birthday = _AddEmployeeDTO.Birthday,
                PhoneNumber = _AddEmployeeDTO.PhoneNumber,
                CIN = _AddEmployeeDTO.CIN,
                Email = _AddEmployeeDTO.Email,
                Username = _AddEmployeeDTO.Username,
                Salaire = _AddEmployeeDTO.Salaire,
                Password = _AddEmployeeDTO.Password,
            };
        }
        public static Employer UpdateEmployeeDTOToEmployer(UpdateEmployeeDTO _UpdateEmployeeDTO)
        {
            return new Employer
            {
                Id = _UpdateEmployeeDTO.Id,
                Nom = _UpdateEmployeeDTO.Nom,
                Prenom = _UpdateEmployeeDTO.Prenom,
                Birthday = _UpdateEmployeeDTO.Birthday,
                PhoneNumber = _UpdateEmployeeDTO.PhoneNumber,
                Email = _UpdateEmployeeDTO.Email,
                Username = _UpdateEmployeeDTO.Username,
                Salaire = _UpdateEmployeeDTO.Salaire,

            };
        }

        public static ReturnAddedEmployee EmployeeToRTE(Employer _Employer)
        {
            return new ReturnAddedEmployee
            {
                Id = _Employer.Id,
                Nom = _Employer.Nom,
                Prenom = _Employer.Prenom,
                Birthday = _Employer.Birthday,
                PhoneNumber = _Employer.PhoneNumber,
                Email = _Employer.Email,
                Username = _Employer.Username,
                Salaire = _Employer.Salaire,
                IdAdmin = _Employer.IdAdmin,
                errors = new List<string>()
            };
        }

    }
}
