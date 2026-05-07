using Application.Interfaces;
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


    }
}
