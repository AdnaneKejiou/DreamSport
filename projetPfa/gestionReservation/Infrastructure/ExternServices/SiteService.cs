using gestionReservation.Core.Interfaces;
using gestionReservation.Infrastructure.ExternServices.Extern_DTo;
using Microsoft.Extensions.Caching.Memory;

namespace gestionReservation.Infrastructure.ExternServices
{
    public class SiteService : ISiteService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://localhost:5150/api";
        public SiteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TerrainDTO> FetchTerrainAsync(int idTerrain) // communiquer avec  gestion SIte
        {
            var response = await _httpClient.GetAsync(SiteUrl+"/Terrain/by-id/"+ idTerrain);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TerrainDTO>() ?? new TerrainDTO();
        }
    }
}
