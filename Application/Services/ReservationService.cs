using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Interfaces;
using Application.Models;
using Application.UseCases.Reservations.Commands;

namespace Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ReservationDto> CreateReservationAsync(Guid seatId, int userId)
        {
            try {
                var command = new CreateReservationCommand(seatId, userId);
                var reservation = await _reservationRepository.CreateReservationAsync(command);

                // AUDITORÍA DE ÉXITO
                await _reservationRepository.CreateAuditLogAsync(
                    new CreateAuditLogCommand(
                        userId,
                        "RESERVE_SUCCESS",
                        "Reservation",
                        reservation.Id.ToString(),
                        $"Butaca {seatId} reservada correctamente"
                    )
                );

                return new ReservationDto
                {
                    Id = reservation.Id,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId,
                    Status = reservation.Status,
                    ReservedAt = reservation.ReservedAt,
                    ExpiresAt = reservation.ExpiresAt
                };

            }


            catch (InvalidOperationException ex)
            {
                await _reservationRepository.CreateAuditLogAsync(
                    new CreateAuditLogCommand(
                        userId,
                        "RESERVE_ATTEMPT_CONFLICT",
                        "Seat",
                        seatId.ToString(),
                        ex.Message
                    )
                );

                throw;
            }

        }
    }
}