using gestionReservation.API.DTOs;
using gestionReservation.API.Exceptions;
using gestionReservation.API.Mappers;
using gestionReservation.Core.Models;
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
        [HttpGet]
        public async Task<ActionResult<List<ReturnedListReservationsDTO>>> GetReservations( [FromQuery] GetReservationsDTO filter)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsAsync( filter);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
            [HttpPost]
        public async Task<IActionResult> AjouterReservationAsync([FromBody] AddReservationDto reservation)
        {
            try
            {
                Reservation res = ReservationMapper.AddDTOtoModel(reservation);
                var result = await _reservationService.AjouterReservationAsync(res);
                return Ok(result); // Return HTTP 200 with the created reservation
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    reservation = reservation
                }); // HTTP 400
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message,
                    reservation = reservation
                }); // HTTP 401
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message,
                    reservation = reservation
                }); // HTTP 404
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred",
                    details = ex.Message,
                    reservation = reservation
                }); // HTTP 500
            }
        }

    }

}
