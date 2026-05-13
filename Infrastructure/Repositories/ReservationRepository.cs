using Application.Interfaces;
using Application.Models;
using Application.UseCases.Reservations.Commands;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            seat.Version++; // Al cambiar esto, cualquier otro proceso con la versión vieja va a fallar al guardar

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

        public async Task CreateAuditLogAsync(CreateAuditLogCommand command)
        {
            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                Action = command.Action,
                EntityType = command.EntityType,
                EntityId = command.EntityId,
                Details = command.Details,
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(audit);

            await _context.SaveChangesAsync();
        }

        public async Task<BulkPaymentConfirmationDto> ConfirmBulkPaymentAsync(List<Guid> reservationIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reservations = await _context.Reservations
                    .Include(r => r.Seat)
                        .ThenInclude(s => s.Sector)
                    .Where(r => reservationIds.Contains(r.Id))
                    .ToListAsync();

                if (reservations.Count != reservationIds.Count)
                    throw new KeyNotFoundException("Una o más reservas no fueron encontradas");

                var now = DateTime.UtcNow;
                var confirmations = new List<PaymentConfirmationDto>();
                decimal total = 0;

                foreach (var reservation in reservations)
                {
                    if (reservation.Status != "Pending")
                        throw new InvalidOperationException($"La reserva {reservation.Id} no está en estado pendiente");

                    if (reservation.ExpiresAt < now)
                        throw new InvalidOperationException($"La reserva {reservation.Id} ha expirado");

                    reservation.Status = "Paid";
                    reservation.Seat.Status = "Sold";
                    total += reservation.Seat.Sector?.Price ?? 0;

                    _context.AuditLogs.Add(new AuditLog
                    {
                        Id = Guid.NewGuid(),
                        UserId = reservation.UserId,
                        Action = "PAYMENT_SUCCESS",
                        EntityType = "Reservation",
                        EntityId = reservation.Id.ToString(),
                        Details = $"Pago confirmado para reserva {reservation.Id}, butaca {reservation.SeatId}",
                        CreatedAt = now
                    });

                    confirmations.Add(new PaymentConfirmationDto
                    {
                        ReservationId = reservation.Id,
                        SeatId = reservation.SeatId,
                        Status = reservation.Status,
                        PaidAt = now
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new BulkPaymentConfirmationDto
                {
                    Confirmations = confirmations,
                    Total = total
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
