using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid SeatId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
