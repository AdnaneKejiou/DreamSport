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
        public static void UpdateUser(User user, UpdateUserDto dto)
        {
            user.Nom = user.Nom;
            user.Prenom = user.Prenom;
            user.Email = user.Email;
            user.Password =  user.Password;
            user.PhoneNumber = user.PhoneNumber;
            user.Username = user.Username;
            user.Birthday = user.Birthday;
            user.Genre =  user.Genre;
        }
        public static void UpdateSportData(User user, UpdateSportDataDTO dto)
        {
           
            user.Bio = dto.Bio;
            user.ImageUrl = dto.ImageUrl;

        }

        //Recup password
        public static User RecupererPasswod(  RecupererPasswordDTO dto)
        {
            return new User {

                Email = dto.Email,
                IdAdmin = dto.idAdmin,
            };
        }

        public static ReturnForgotPasswordDTO returnUpdatedPasswordDTO(User user)
        {
            return new ReturnForgotPasswordDTO
            {
                Email = user.Email,
                idAdmin = user.IdAdmin
            };
        }

    }
}
