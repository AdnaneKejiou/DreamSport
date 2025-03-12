using gestionEmployer.API.DTOs.AdminDTO;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IAdminService
    {

        bool ValidateTenant(int tenantId);
        Admin? GetAdmin(int tenantId);
        AdminAddedDTO AjouterAdmin(Admin admin);
        Task<SendLoginEmployeeDto> ValidateLoginAsync(AdminLoginDto dto);
    }
}