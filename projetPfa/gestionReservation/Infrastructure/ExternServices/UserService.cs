using gestionReservation.Core.Interfaces;
using gestionReservation.Infrastructure.ExternServices.Extern_DTo;

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
    }
}
