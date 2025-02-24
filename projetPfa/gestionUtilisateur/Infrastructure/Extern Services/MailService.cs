using gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs;
using System.Text.Json;
using System.Text;
using gestionUtilisateur.Core.Interfaces;

namespace gestionUtilisateur.Infrastructure.Extern_Services
{
    public class MailService : IMailService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://localhost:5193/api/Mail/send";

        public MailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> MailRecoverkey(EmailRequest request)
        {
                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            
                HttpResponseMessage response = await _httpClient.PostAsync(SiteUrl, jsonContent);
                return response.IsSuccessStatusCode;
        }
    }
}
