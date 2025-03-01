using gestionEmployer.API.DTOs.AdminDTO;
using gestionEmployer.Core.Models;

namespace gestionEmployer.API.Mappers
{
    public class AdminMapper
    {
        public static Admin AdminDTOToAdmin(AjouterAdminDTO dto)
        {
            return new Admin
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Login = dto.Login
            };
        }

        public static AdminAddedDTO AdminToAdminAddedDTO(Admin admin)
        {
            return new AdminAddedDTO
            {
                Id = admin.Id,
                Nom = admin.Nom,
                Prenom = admin.Prenom,
                Login = admin.Login
            };
        }
    }
}
