
using gestionSite.Core.Interfaces.TerrainInterfaces;
using Microsoft.EntityFrameworkCore;
using gestionSite.Core.Models;
using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;

namespace gestionSite.Core.Services
{
    public class TerrainService : ITerrainService
    {
        private readonly ITerrainRepository _terrainRepository;
        private readonly ITerrainStatusRepository _terrainStatusRepository;

        public TerrainService(ITerrainRepository terrainRepository, ITerrainStatusRepository terrainStatusRepository)
        {
            _terrainRepository = terrainRepository;
            _terrainStatusRepository = terrainStatusRepository; 
        }

        // Récupérer tous les terrains associés à un administrateur
        public async Task<IEnumerable<Terrain>> GetAllTerrainsByAdminAsync(int idAdmin)
        {
            // Validation ou logique supplémentaire peut être ajoutée ici
            return await _terrainRepository.GetAllTerrainsByAdminAsync(idAdmin);
        }

        // Ajouter un terrain
        public async Task<Terrain?> AddTerrainAsync(Terrain terrain)
        {
            // Logique de validation avant l'ajout
            if (await _terrainRepository.ExistsAsync(terrain.Title, terrain.IdAdmin))
            {
                throw new InvalidOperationException("A terrain with this title already exists.");
            }
            

            return await _terrainRepository.AddTerrainAsync(terrain);
        }

        // Mettre à jour un terrain
        public async Task<Terrain?> UpdateTerrainAsync(Terrain terrain)
        {
            // Vérifier que le terrain existe avant la mise à jour
            var existingTerrain = await _terrainRepository.GetTerrainByIdAsync(terrain.Id);
            if (existingTerrain == null)
            {
                throw new KeyNotFoundException("Terrain not found.");
            }

            return await _terrainRepository.UpdateTerrainAsync(terrain);
        }

        // Supprimer un terrain
        public async Task<Terrain?> DeleteTerrainAsync(int id)
        {
            var existingTerrain = await _terrainRepository.GetTerrainByIdAsync(id);
            if (existingTerrain == null)
            {
                return null; // Terrain non trouvé
            }

            return await _terrainRepository.DeleteTerrainAsync(id);
        }

        // Vérifier si un terrain existe par son titre pour un administrateur donné
        public async Task<bool> ExistsAsync(string title, int idAdmin)
        {
            return await _terrainRepository.ExistsAsync(title, idAdmin);
        }

        // Récupérer un terrain par son ID
        public async Task<Terrain?> GetTerrainByIdAsync(int id)
        {
            return await _terrainRepository.GetTerrainByIdAsync(id);
        }

        public async Task<Terrain?> GetTerrainByIdWithStatusAsync(int id)
        {
            return await _terrainRepository.GetTerrainByIdWithStatusAsync(id);
        }

        public async Task<Terrain?> UpdateTerrainStatusAsync(UpdateStatusDto dto)
        {
            Terrain terrain = await _terrainRepository.GetTerrainByIdAsync(dto.Id);
            if (terrain == null)
            {
                throw new KeyNotFoundException("The court not found ");
            }
            if(await _terrainStatusRepository.ExistsByIdAsync(dto.statusId) == null)
            {
                throw new KeyNotFoundException("The Status you provided not found ");
            }
            terrain.TerrainStatusId = dto.statusId;
            return await _terrainRepository.UpdateTerrainAsync(terrain);
        }
    }
}
