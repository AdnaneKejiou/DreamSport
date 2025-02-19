using gestionReservation.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace gestionReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDto reservationDto)
        {
            if (reservationDto == null)
            {
                return BadRequest("La réservation est invalide.");
            }

            bool result = await _reservationService.CreateReservationAsync(reservationDto);

            if (result)
            {
                return Ok("Réservation créée avec succès.");
            }
            else
            {
                return BadRequest("La réservation a échoué. L'utilisateur peut être bloqué.");
            }
        }
    }

}
