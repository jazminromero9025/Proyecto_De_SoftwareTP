using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        // El constructor es necesario para que la API le pase la configuración (como la conexión a la DB)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        // Estas son las "tablas". Entity Framework usará estas propiedades para crear la DB.
        public DbSet<Event> Events { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Concurrencia para la butaca usando el int del diagrama
            modelBuilder.Entity<Seat>()
                .Property(s => s.Version)
                .IsConcurrencyToken();

            // Configuración de precisión para el precio
            modelBuilder.Entity<Sector>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);
        }







    }



    
}
