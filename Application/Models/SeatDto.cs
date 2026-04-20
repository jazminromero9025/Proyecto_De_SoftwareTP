using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class SeatDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }

        public string Status { get; set; } = string.Empty;

       

    }
}
