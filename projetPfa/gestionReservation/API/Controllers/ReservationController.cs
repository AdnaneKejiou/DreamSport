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

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateReservationStatus([FromBody] UpdateStatusDTO dto)
        {
            try
            {
                Reservation reservation = await _reservationService.ReservationStatusUpdateAsync(dto);
                return Ok(reservation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}
