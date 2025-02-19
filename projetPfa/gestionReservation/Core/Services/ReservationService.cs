using gestionReservation.API.DTOs;
using gestionReservation.Core.Models;
using System.Net.Http;
using System.Text.Json;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IStatusService _statusService;
    private readonly HttpClient _httpClient; // Client HTTP pour les appels API

    public ReservationService(
        IReservationRepository reservationRepository,
        IStatusService statusService,
        HttpClient httpClient)
    {
        _reservationRepository = reservationRepository;
        _statusService = statusService;
        _httpClient = httpClient;
    }

    // Crée une réservation
    public async Task<bool> CreateReservationAsync(ReservationDto reservationDto)
    {
        // Vérifier si l'utilisateur est bloqué
        bool isBlocked = await IsReservationBlocked(reservationDto.IdUtilisateur);
        if (isBlocked)
        {
            return false;  // L'utilisateur est bloqué, il ne peut pas réserver
        }

        // Vérifier l'existence du terrain
        if (!await DoesTerrainExistAsync(reservationDto.IdTerrain))
        {
            return false;
        }

        // Récupérer le statut "En attente" par défaut
        var defaultStatus = await _statusService.GetDefaultStatusAsync();
        if (defaultStatus == null)
        {
            return false; // Si le statut "En attente" n'existe pas, la réservation échoue
        }

        // Créer une nouvelle réservation et affecter les propriétés du DTO à l'entité Reservation
        var reservationEntity = new Reservation
        {
            DateRes = reservationDto.DateRes,
            IdUtilisateur = reservationDto.IdUtilisateur,
            IdTerrain = reservationDto.IdTerrain,
            IdEmploye = reservationDto.IdEmploye,
            IdAdmin = reservationDto.IdAdmin,
            Status = defaultStatus // Assigner le statut "En attente"
        };

        // Ajouter la réservation dans la base de données
        await _reservationRepository.AddAsync(reservationEntity);

        return true;  // Réservation réussie
    }

    // Vérifie si l'utilisateur est bloqué
    private async Task<bool> IsReservationBlocked(int userId)
    {
        var response = await _httpClient.GetAsync($"http://user-service/api/users/{userId}/status");
        if (!response.IsSuccessStatusCode)
        {
            return true; // Si l'appel échoue, on considère que l'utilisateur est bloqué
        }

        var userStatus = await response.Content.ReadAsStringAsync(); // Exemple : "blocked" ou "active"
        return userStatus.Equals("blocked", StringComparison.OrdinalIgnoreCase);
    }

    // Vérifie l'existence du terrain
    private async Task<bool> DoesTerrainExistAsync(int terrainId)
    {
        var response = await _httpClient.GetAsync($"http://terrain-service/api/terrains/{terrainId}");
        return response.IsSuccessStatusCode;
    }
}
