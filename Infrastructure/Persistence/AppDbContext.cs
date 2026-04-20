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

            // 1. RELACIONES (Las "flechitas" del diagrama)

            // EVENTO -> SECTOR
            modelBuilder.Entity<Sector>()
                .HasOne(s => s.Event)
                .WithMany(e => e.Sectors)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // SECTOR -> SEAT
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Sector)
                .WithMany(sec => sec.Seats)
                .HasForeignKey(s => s.SectorId)
                .OnDelete(DeleteBehavior.Cascade);

            // SEAT -> RESERVATION (Se asigna a)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Seat)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // USER -> RESERVATION (Realiza)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // USER -> AUDIT_LOG (Genera)
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // 2. CONFIGURACIONES ESPECIALES (Precisiones y Concurrencia)
            modelBuilder.Entity<Seat>()
                .Property(s => s.Version)
                .IsConcurrencyToken();

            modelBuilder.Entity<Sector>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            // 3. NOMBRES DE TABLAS EN SINGULAR (Identico al diagrama del profe)
            modelBuilder.Entity<Event>().ToTable("EVENT");
            modelBuilder.Entity<Sector>().ToTable("SECTOR");
            modelBuilder.Entity<Seat>().ToTable("SEAT");
            modelBuilder.Entity<User>().ToTable("USER");
            modelBuilder.Entity<Reservation>().ToTable("RESERVATION");
            modelBuilder.Entity<AuditLog>().ToTable("AUDIT_LOG");

            //precargade datos abajo
           
        }







    }



    
}
