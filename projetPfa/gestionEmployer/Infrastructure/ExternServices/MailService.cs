using gestionEmployer.Core.Interfaces;
using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;
using System.Text.Json;
using System.Text;

namespace gestionEmployer.Infrastructure.ExternServices
{
    public class MailService : IMailService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://localhost:5193/api/Mail/send";

        public MailService() 
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> NewEmployeeMail(EmailRequest request)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PostAsync(SiteUrl, jsonContent);
            return response.IsSuccessStatusCode;
        }
    }
}
