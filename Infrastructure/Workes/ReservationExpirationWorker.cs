using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Infrastructure.Workes
{

     

    public class ReservationExpirationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        public ReservationExpirationWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // 1. Iniciamos la transacción para que todo sea "todo o nada"
                    using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);

                    try
                    {
                        var now = DateTime.UtcNow;
                        var expiredReservations = await context.Reservations
                            .Include(r => r.Seat)
                            .Where(r => r.Status == "Pending" && r.ExpiresAt < now)
                            .ToListAsync(stoppingToken);

                        if (expiredReservations.Any())
                        {
                            foreach (var res in expiredReservations)
                            {
                                res.Status = "Expired";
                                if (res.Seat != null)
                                {
                                    res.Seat.Status = "Available";
                                    res.Seat.Version++; 
                                }

                                context.AuditLogs.Add(new AuditLog
                                {
                                    Id = Guid.NewGuid(),
                                    Action = "AUTO_RELEASE", 
                                    Details = $"Liberación automática de butaca {res.SeatId}",
                                    CreatedAt = now
                                });
                            }

                            await context.SaveChangesAsync(stoppingToken);

                            // 2. Si llegamos acá sin errores, confirmamos los cambios
                            await transaction.CommitAsync(stoppingToken);
                        }
                    }
                    catch (Exception)
                    {
                        // 3. Si algo falló, deshacemos todo para no dejar datos inconsistentes
                        await transaction.RollbackAsync(stoppingToken);
                        
                    }
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }






}
