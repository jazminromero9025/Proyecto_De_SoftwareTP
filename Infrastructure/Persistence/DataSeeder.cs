using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public static class DataSeeder
        {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            { 

                // ── USUARIOS ────────────────────────────────────────────────
                var users = new List<User>
              {
                new User { Name = "Juan Pérez", Email = "juan@test.com", Role = "Customer",  PasswordHash = "hash123" },
                new User { Name = "María García", Email = "maria@test.com", Role = "Customer",  PasswordHash = "hash123" },
                new User { Name = "Carlos López", Email = "carlos@test.com", Role= "Administrador",  PasswordHash = "hash123" },
              };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
            if (context.Events.Any()) return;

            // ── EVENTO ──────────────────────────────────────────────────
            var evento = new Event
            {
                
                Name = "Concierto de Rock",
                EventDate = DateTime.UtcNow.AddDays(30),
                Venue = "Estadio River Plate",
                Status = "Active",
                Sectors = new List<Sector>()
            };

            // ── SECTORES Y BUTACAS ──────────────────────────────────────
            var sectoresConfig = new[]
            {
                new { Name = "Sector A", Price = 5000m, Capacity = 50 },
                new { Name = "Sector B", Price = 3000m, Capacity = 50 }
            };

            foreach (var config in sectoresConfig)
            {
                var sector = new Sector
                {
                    Name = config.Name,
                    Price = config.Price,
                    Capacity = config.Capacity,
                    Seats = new List<Seat>()
                };

                for (int i = 1; i <= 50; i++)
                {
                    sector.Seats.Add(new Seat
                    {
                        Id = Guid.NewGuid(),
                        RowIdentifier = i <= 25 ? "A" : "B",
                        SeatNumber = i,
                        Status = "Available",
                        Version = 0
                    });
                }

                evento.Sectors.Add(sector);
            }

            context.Events.Add(evento);
            context.SaveChanges();
        }
    }
}
