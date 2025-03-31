using gestionEmployer.API.DTOs.AdminDTO;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.API.Mappers;
using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Infrastructure.Data.Repositories;
using Shared.Messaging.Services;

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
        public AdminAddedDTO AjouterAdmin(Admin admin)
        {
            AdminAddedDTO _adminDTO = AdminMapper.AdminToAdminAddedDTO(admin);

            if (_adminRepository.AdminExists(admin.Nom, admin.Login, admin.PhoneNumber))
            {
                _adminDTO.errors.Add("An administrator with this name, login, or phone number already exists");
                return _adminDTO;
            }

            var adminAdded = _adminRepository.AddAdmin(admin);

            if (adminAdded == null)
            {
                _adminDTO.errors.Add("An error occurred while adding the administrator. Please try again.");
                return _adminDTO;
            }

            _adminDTO = AdminMapper.AdminToAdminAddedDTO(adminAdded);

            var _producer = new RabbitMQProducerService("Creation de site");
            // Utilisation de RabbitMQ Producer injecté
            _producer.Publish(new { Id = _adminDTO.Id, Name = _adminDTO.Nom });

            return _adminDTO;
        }

        public async Task<SendLoginEmployeeDto> ValidateLoginAsync(AdminLoginDto dto)
        {
            Admin admin = await _adminRepository.GetByLoginAsync(dto.Login);
            if(admin == null)
            {
                throw new KeyNotFoundException("Tenant not Found");
            }
            if (!admin.Password.Equals(dto.Password))
            {
                return null;
            }
            return AdminMapper.ModelToLogin(admin);
        }

       //public async Task<ReturnUpdatedAdminDto?> UpdateAdminAsync(Admin updatedAdmin)
       // {
       //     // Récupérer l'employé existant
       //     Admin existingAdmin = _adminRepository.GetAdminByTenantId(updatedAdmin.Id)
       //                             ?? throw new KeyNotFoundException("Admin non trouvé.");

       //     // Liste pour stocker les erreurs trouvées
       //     ReturnUpdatedAdminDto dto = AdminMapper.ModelToUpdate(updatedAdmin);

       //     // Vérification de l'unicité seulement si l'attribut est modifié
       //     if (updatedAdmin.Login != null && !updatedAdmin.Login.Equals(existingAdmin.Login))
       //     {
       //         if (await _adminRepository.GetByLoginAsync(updatedAdmin.Login, updatedAdmin.Id) != null)
       //         {
       //             {
       //                 dto.Errors.Add("Login", "Login already exist.");
       //             }
       //         }
       //     }
       //     if (updatedAdmin.PhoneNumber != null && !updatedAdmin.PhoneNumber.Equals(existingAdmin.PhoneNumber))
       //     {
       //         if (await _adminRepository.EmployerByPhoneAsync(updatedAdmin.PhoneNumber, updatedAdmin.Id) != null)
       //         {
       //             dto.Errors.Add("PhoneNumber", "PhoneNumber already exist.");
       //         }
       //     }

       //     if (dto.Errors.Count() == 0)
       //     {
       //         EmployeeMapper.updateToModel(existingEmploye, updatedEmploye);
       //         // Sauvegarder les changements dans la base de données
       //         var empp = await _employeeRepository.UpdateEmployeeAsync(existingEmploye);
       //     }

       //     return dto;
       // }
    }


}
