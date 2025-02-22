using gestionReservation.API.DTOs;
using gestionReservation.Core.Interfaces;
using gestionReservation.Core.Models;
using gestionReservation.API.Exceptions;
using System.Net.Http;
using System.Text.Json;
using gestionReservation.Infrastructure.Data.Repositories;
using gestionReservation.API.Mappers;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ISiteService _siteService;
    private readonly IUserService _userService;
    public ReservationService(IReservationRepository reservationRepository, IStatusRepository statusRepository, IUserService userService, ISiteService siteService)
    {
        _reservationRepository = reservationRepository;
        _statusRepository = statusRepository;
        _userService = userService; 
        _siteService = siteService;
    }

    public async Task<AddReservationDto> AjouterReservationAsync(Reservation reservation)
    {
        if(reservation.DateRes < DateTime.Now)
        {
            throw new BadRequestException("The reservation date must be today or in the future.");
        }
        var user = await _userService.FetchUserAsync(reservation.IdUtilisateur);
        if(user == null)
        {
            throw new KeyNotFoundException("The user dont exist in our sytem");
        }
        if(user.IsReservationBlocked)
        {
            throw new UnauthorizedAccessException("You cant Reserve please talk to the support");
        }
        var terrain = await _siteService.FetchTerrainAsync(reservation.IdUtilisateur);
        if(terrain == null)
        {
            throw new KeyNotFoundException("The terrain dont exist in our sytem");
        }
        if(terrain.terrainStatus.Libelle!= "Disponible")
        {
            throw new BadRequestException("This terrain cant be reserved at the moment");
        }
        if (await _reservationRepository.GetReservationsCountByTerrainAndDateAsync(reservation.IdTerrain, reservation.DateRes) > 0)
        {
            throw new BadRequestException("This terrain is reserved at the date provided");
        }
        Status st = await _statusRepository.GetStatusByLibelle("en attente");
        int idStatus = st.Id;
        reservation.IdStatus = idStatus;

        await _reservationRepository.AddAsync(reservation);//add reservation
        
        var dto = ReservationMapper.ModelToAddDTO(reservation);
        return dto;
    }
    public async Task<List<ReturnedListReservationsDTO>> GetReservationsAsync( GetReservationsDTO filter)
    {
     

        var reservations = await _reservationRepository.GetReservationsAsync(filter.StartDate, filter.EndDate);
        List < ReturnedListReservationsDTO > reservationdto = ReservationMapper.ReservationsToReservationsDTOs(reservations);
        return reservationdto;
    }



}
