using gestionReservation.API.DTOs;
using gestionReservation.Core.Models;

namespace gestionReservation.API.Mappers
{
    public class ReservationMapper
    {
        public static AddReservationDto ModelToAddDTO(Reservation reservation)
        {
            return new AddReservationDto
            {
                DateRes = reservation.DateRes,
                IdUtilisateur = reservation.IdUtilisateur,
                IdTerrain = reservation.IdTerrain,
                IdAdmin = reservation.IdAdmin,
            };
        }

        public static Reservation AddDTOtoModel(AddReservationDto dto)
        {
            return new Reservation
            {
                DateRes = dto.DateRes,
                IdAdmin = dto.IdAdmin,
                IdTerrain = dto.IdTerrain,
                IdUtilisateur = dto.IdUtilisateur,

            };
        }
    }
}
