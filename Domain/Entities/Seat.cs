using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Seat
    {
        public Guid Id { get; set; } //PK

        public int SectorId { get; set; } //FK

        public string RowIdentifier { get; set; } = string.Empty;
        public int SeatNumber { get; set; }

        public string Status { get; set; } = string.Empty; // Available, Reserved, Sold

        public int Version { get; set; } // Control de concurrencia


        // Propiedad de navegación (La que dibuja la línea en SQL)
        public virtual Sector Sector { get; set; } = null!;

        // Una butaca puede estar en muchas reservas (historial)
        public virtual List<Reservation> Reservations { get; set; } = [];

    }
}
