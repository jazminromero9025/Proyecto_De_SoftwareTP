using Application.Interfaces;
using Application.Models;
using Application.UseCases.Reservations.Commands;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> CreateReservationAsync(CreateReservationCommand command)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.Id == command.SeatId);

            if (seat == null)
                throw new KeyNotFoundException("Butaca no encontrada");

            if (seat.Status != "Available")
                throw new InvalidOperationException("La butaca no está disponible");

            seat.Status = "Reserved";
            seat.Version++;

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                SeatId = command.SeatId,
                UserId = command.UserId,
                Status = "Pending",
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            _context.Reservations.Add(reservation);

            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                Action = "RESERVE_SUCCESS",
                EntityType = "Reservation",
                EntityId = reservation.Id.ToString(),
                Details = $"Butaca {command.SeatId} reservada por usuario {command.UserId}",
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(audit);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("Conflicto de concurrencia: la butaca ya fue reservada");
            }

            return reservation;
        }

        public async Task<PaymentConfirmationDto> ConfirmPaymentAsync(Guid reservationId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reservation = await _context.Reservations
                    .Include(r => r.Seat)
                    .FirstOrDefaultAsync(r => r.Id == reservationId);

                if (reservation == null)
                    throw new KeyNotFoundException("Reserva no encontrada");

                if (reservation.Status != "Pending")
                    throw new InvalidOperationException("La reserva no está en estado pendiente");

                if (reservation.ExpiresAt < DateTime.UtcNow)
                    throw new InvalidOperationException("La reserva ha expirado");

                reservation.Status = "Paid";
                reservation.Seat.Status = "Sold";

                var audit = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = reservation.UserId,
                    Action = "PAYMENT_SUCCESS",
                    EntityType = "Reservation",
                    EntityId = reservation.Id.ToString(),
                    Details = $"Pago confirmado para reserva {reservation.Id}, butaca {reservation.SeatId}",
                    CreatedAt = DateTime.UtcNow
                };

                _context.AuditLogs.Add(audit);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new PaymentConfirmationDto
                {
                    ReservationId = reservation.Id,
                    SeatId = reservation.SeatId,
                    Status = reservation.Status,
                    PaidAt = DateTime.UtcNow
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
