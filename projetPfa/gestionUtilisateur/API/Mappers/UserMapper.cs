using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.Core.Models;

namespace gestionUtilisateur.API.Mappers
{
    public class UserMapper
    {
        public static User AddUserToUser(AddUserManualyDTO dto)  //to pass data from controller to service layer
        {
            return new User
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Username = dto.Username,
                Birthday = dto.Birthday,
                Genre = dto.Genre,
                IdAdmin = dto.IdAdmin,
            };
        }

        public static ReturnAddedUserManualy UserToAddedUser(User user) {
            return new ReturnAddedUserManualy
            {
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Username = user.Username,
                Birthday = user.Birthday,
                Genre = user.Genre,
                IdAdmin = user.IdAdmin,
            };
        }
    }
}
