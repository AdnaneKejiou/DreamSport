using Auth.Dtos;
using Auth.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Auth.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://localhost:5010/gateway";
        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> LoginAdminAsync(AdminLoginDto userLogin)
        {
            int adminId = userLogin.AdminId; // Example Admin ID to be sent in the header

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/admin/validate";

            // Create the request with headers
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers
            var jsonContent = new StringContent(JsonConvert.SerializeObject(new { userLogin.Login, userLogin.Password }), Encoding.UTF8, "application/json");
            request.Content = jsonContent;

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("Tenant with this Login not found");
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Login or Password are incorrect");
            }
            if (!response.IsSuccessStatusCode)
            {
                string errorMsg = $"❌ Error fetching user: {response.StatusCode}, URL: {requestUrl}";
                return -1;
            }

            int id = await response.Content.ReadFromJsonAsync<int>();
            return id;
        }
    }
}
