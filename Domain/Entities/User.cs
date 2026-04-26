using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; } //pk
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer"; // Le ponemos un valor por defecto para que no falle

        // Relación: Un usuario realiza muchas reservas
        public virtual List<Reservation> Reservations { get; set; } = [];

        public virtual List<AuditLog> AuditLogs { get; set; } = [];
    }
}
