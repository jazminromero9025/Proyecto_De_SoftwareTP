using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public int UserId { get; set; } // FK

        public virtual User User { get; set; } = null!; // Propiedad de navegación
        public Guid SeatId { get; set; } // FK
        public string Status { get; set; } = string.Empty;// Pending, Paid, Expired

        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        // Propiedad de navegación:
        public virtual Seat Seat { get; set; } = null!;

    }
}
