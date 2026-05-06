using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequest request)
        {
            try
            {
                var reservation = await _reservationService.CreateReservationAsync(
                    request.SeatId,
                    request.UserId
                );
                return Ok(reservation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("{reservationId}/confirm-payment")]
        public async Task<IActionResult> ConfirmPayment(Guid reservationId)
        {
            try
            {
                var result = await _reservationService.ConfirmPaymentAsync(reservationId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }

    public class CreateReservationRequest
    {
        public Guid SeatId { get; set; }
        public int UserId { get; set; }
    }
}
