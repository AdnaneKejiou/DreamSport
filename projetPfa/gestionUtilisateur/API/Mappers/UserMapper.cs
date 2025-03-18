using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.Core.Models;

namespace gestionUtilisateur.API.Mappers
{
    public class UserMapper
    {
        public static User AddUserToUser(AddUserManualyDTO dto)  //to pass data from controller to service layer
        {

            User user = new User();

            user.Nom = dto.Nom;
            user.Prenom = dto.Prenom;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.PhoneNumber = dto.PhoneNumber;
            user.Username = dto.Username;
            if (dto.Birthday!=null)
            {
                user.Birthday = dto.Birthday;

            }
            user.Genre = dto.Genre;
            user.IdAdmin = dto.AdminId;

            return user;
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
                AdminId = user.IdAdmin,
            };
        }
        public static void UpdateUser(User user, UpdateUserDto dto)
        {
            if (dto.Nom != null)
            {
                user.Nom = dto.Nom;
            }

            if (dto.Prenom != null)
            {
                user.Prenom = dto.Prenom;
            }

            if (dto.Email != null)
            {
                user.Email = dto.Email;
            }

            if (dto.Password != null)
            {
                user.Password = dto.Password;
            }
            if (dto.Birthday != null)
            {
                user.Birthday = dto.Birthday;
            }

            if (dto.PhoneNumber != null)
            {
                user.PhoneNumber = dto.PhoneNumber;
            }

            if (dto.Username != null)
            {
                user.Username = dto.Username;
            }

        }
        public static void UpdateSportData(User user, UpdateSportDataDTO dto)
        {
            if (dto.Bio != null)
            {
                user.Bio = dto.Bio;
            }

            if (dto.ImageUrl != null)
            {
                user.ImageUrl = dto.ImageUrl;
            }

        }

        //Recup password
        public static User RecupererPasswod(  RecupererPasswordDTO dto)
        {
            return new User {

                Email = dto.Email,
                IdAdmin = dto.AdminId,
            };
        }

        public static ReturnForgotPasswordDTO returnUpdatedPasswordDTO(User user)
        {
            return new ReturnForgotPasswordDTO
            {
                Email = user.Email,
                AdminId = user.IdAdmin
            };
        }

    }
}
