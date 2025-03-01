using gestionEmployer.API.DTOs.AdminDTO;
using gestionEmployer.API.Mappers;
using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
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
    }


}
