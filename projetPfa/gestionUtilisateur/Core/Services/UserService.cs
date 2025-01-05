using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.API.Mappers;
using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Models;


namespace gestionUtilisateur.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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



    }
}
