using gestionReservation.Core.Interfaces;
using gestionReservation.Infrastructure.ExternServices.Extern_DTo;
using System.Net;

namespace gestionReservation.Infrastructure.ExternServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://localhost:5010/gateway";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO> FetchUserAsync(int idUser)
        {
            int adminId = 1; // Example Admin ID to be sent in the header

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/{idUser}";

            // Create the request with headers
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers

            Console.WriteLine($"🔍 Requesting: {requestUrl}, with Tenant-ID: {adminId}"); // Debugging Output

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorMsg = $"❌ Error fetching user: {response.StatusCode}, URL: {requestUrl}";
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserDTO>();
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
