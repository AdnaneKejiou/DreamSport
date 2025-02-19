using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.API.Mappers;
using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Models;
using System.Net.Http;
using System.Text.Json;

namespace gestionUtilisateur.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;  // Modifiez ici pour utiliser HttpClient

        // Injecter HttpClient via le constructeur
        public UserService(IUserRepository userRepository, HttpClient httpClient)
        {
            _userRepository = userRepository;
            _httpClient = httpClient;  // Stocker l'instance d'HttpClient
        }

        public async Task<ReturnAddedUserManualy> AddUserManualyAsync(User _user)
        {
            var Errors = new Dictionary<string, string>();
            if (await _userRepository.DoesUserWithPhoneExist(_user.PhoneNumber, _user.IdAdmin))
            {
                Errors.Add("PhoneNumber", "User with that Number already exist");
            }
            if (await _userRepository.DoesUserWithEmailExist(_user.Email, _user.IdAdmin))
            {
                Errors.Add("Email", "User with that Email already exist");
            }
            if (await _userRepository.DoesUserWithUsernameExist(_user.Username, _user.IdAdmin))
            {
                Errors.Add("Username", "User with that Username already exist");
            }
            if (Errors.Count == 0)
            {
                var user = await _userRepository.AddUserManualyAsync(_user);
                var AddedUsers = UserMapper.UserToAddedUser(user);
                if (user == null)
                {
                    AddedUsers.errors.Add("Server", "Some error happen while adding you, please try again later");
                    return AddedUsers;
                }
                return AddedUsers;

            }
            var AddedUser = UserMapper.UserToAddedUser(_user);
            AddedUser.errors = Errors;
            return AddedUser;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            UserMapper.UpdateUser(user, dto);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> UpdateSportDataAsync(int userId, UpdateSportDataDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            UserMapper.UpdateSportData(user, dto);

            await _userRepository.UpdateAsync(user);

            return true;
            //ajouter CDN
        }

        public async Task<ReturnForgotPasswordDTO> RecupererPasswodAsync(RecupererPasswordDTO dto)
        {
            // Recherche l'utilisateur par email
            var user = await _userRepository.GetByEmailAsync(dto.Email, dto.idAdmin);
            if (user == null)
            {
                user = UserMapper.RecupererPasswod(dto);
                var Returnto = UserMapper.returnUpdatedPasswordDTO(user);
                Returnto.error = "Aucun utilisateur trouvé avec cet email";
                return Returnto;
            }
            var ReturnDto = UserMapper.returnUpdatedPasswordDTO(user);

            // Générer un nouveau mot de passe
            var nouveauMotDePasse = GenererNouveauMotDePasse();

            // Mise à jour du mot de passe dans l'objet utilisateur
            user.Password = nouveauMotDePasse;

            // Mise à jour dans la base de données
            await _userRepository.UpdateAsync(user);
            // Retourner true après une mise à jour réussie
            return ReturnDto;
        }

        private string GenererNouveauMotDePasse()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Vérification si l'utilisateur est bloqué
        public async Task<User> GetUserAsync(int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            return user;
        }
    }
}
