using gestionReservation.Core.Interfaces;
using gestionReservation.Infrastructure.ExternServices.Extern_DTo;
using System.Net;

namespace gestionReservation.Infrastructure.ExternServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://localhost:5255/api";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO> FetchUserAsync(int idUser) // communiquer avec  gestion SIte
        {
            var response = await _httpClient.GetAsync(UserUrl + "/User/" + idUser);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserDTO>() ?? new UserDTO();
        }

        public async Task<bool> ResetConteurResAnnulerAsync(int id)
        {
            var response = await _httpClient.PutAsync($"{UserUrl}/User/check-reservation-annule/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return true; // Successfully reset
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false; // User not found
            }

            response.EnsureSuccessStatusCode(); // Throws an exception if status code is 4xx or 5xx
            return false;
        }

        public async Task<bool> CheckAndIncrementReservationAnnuleAsync(int userId)
        {
            var response = await _httpClient.PutAsync($"{UserUrl}/User/check-reservation-annule/{userId}", null);

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

    }
}
