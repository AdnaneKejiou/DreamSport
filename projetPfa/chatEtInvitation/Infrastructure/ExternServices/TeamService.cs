using chatEtInvitation.Core.Interfaces.IExternServices;

namespace chatEtInvitation.Infrastructure.ExternServices
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://localhost:5010/gateway";

        public TeamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> FetchMembersAsync(int TeamId)
        {
            int adminId = 1; // Example Admin ID to be sent in the header

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/equipe/{TeamId}";

            // Create the request with headers
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<int>>() ?? null;
        }
    }
}
