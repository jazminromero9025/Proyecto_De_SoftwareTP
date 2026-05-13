using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class ReservationExpiryService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ReservationExpiryService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ExpireReservationsAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task ExpireReservationsAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var expired = await context.Reservations
                .Include(r => r.Seat)
                .Where(r => r.Status == "Pending" && r.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            if (!expired.Any()) return;

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                foreach (var reservation in expired)
                {
                    reservation.Status = "Expired";
                    reservation.Seat.Status = "Available";

                    context.AuditLogs.Add(new AuditLog
                    {
                        Id = Guid.NewGuid(),
                        UserId = reservation.UserId,
                        Action = "RESERVE_EXPIRED",
                        EntityType = "Reservation",
                        EntityId = reservation.Id.ToString(),
                        Details = $"Reserva {reservation.Id} expirada, butaca {reservation.SeatId} liberada",
                        CreatedAt = DateTime.UtcNow
                    });
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
