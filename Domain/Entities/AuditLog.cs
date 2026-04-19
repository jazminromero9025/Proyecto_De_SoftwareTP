using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public int? UserId { get; set; } // puede ser null si es un proceso del sistema

        public string Action { get; set; } = string.Empty;//ejemplo RESERVE_ATTEMP, RESERVE_SUCESS, EXPIRED
        public string EntityType { get; set; } = string.Empty;//Ejemplo Reservation, Seat
        public string EntityId { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty; // JSON con metadatos del evento
        public DateTime CreatedAt { get; set; }

    }
}
