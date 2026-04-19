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
        public Guid SeatId { get; set; } // FK
        public string Status { get; set; } = string.Empty;// Pending, Paid, Expired

        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }


    }
}
