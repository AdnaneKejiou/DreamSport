using gestionEquipe.Infrastructure.ExternServices.ExternDTO;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using gestionEquipe.Core.Interfaces;

namespace gestionEquipe.Infrastructure.ExternServices
{
    public class SiteService : ISiteService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "SportCategories";

        public SiteService(HttpClient httpClient, IMemoryCache cacheService)
        {
            _httpClient = httpClient;
            _cache = cacheService;
        }

        public async Task<List<SportCategorieDTO>> GetSportsAsync()
        {
            // Check if data is in cache
            if (_cache.TryGetValue(CacheKey, out List<SportCategorieDTO> cachedData))
            {
                return cachedData;
            }

            // Ila kan lcashe khawi
            var result = await FetchSportsAsync();

            // Cache the data for 5 minutes
            _cache.Set(CacheKey, result, TimeSpan.FromMinutes(60));
            Console.WriteLine(result);
            return result;
        }

        private async Task<List<SportCategorieDTO>> FetchSportsAsync() // communiquer avec  gestion SIte
        {
            var response = await _httpClient.GetAsync("http://localhost:5150/api/SportCategorie/execute");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<SportCategorieDTO>>() ?? new List<SportCategorieDTO>();
        }

    }
}
