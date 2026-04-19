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


    }
}
