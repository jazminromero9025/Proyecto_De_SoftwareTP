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
            var command = new CreateReservationCommand(seatId, userId);
            var reservation = await _reservationRepository.CreateReservationAsync(command);

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

        public async Task<PaymentConfirmationDto> ConfirmPaymentAsync(Guid reservationId)
        {
            return await _reservationRepository.ConfirmPaymentAsync(reservationId);
        }

        public async Task<BulkPaymentConfirmationDto> ConfirmBulkPaymentAsync(List<Guid> reservationIds)
        {
            return await _reservationRepository.ConfirmBulkPaymentAsync(reservationIds);
        }
    }
}