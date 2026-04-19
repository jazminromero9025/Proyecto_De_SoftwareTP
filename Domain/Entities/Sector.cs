using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sector
    {
        public int Id { get; set; } //pk
        public int EventId { get; set; } // FK hacia Evento



    }
}
