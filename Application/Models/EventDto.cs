using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class EventDto
    {
        public int Id { get; set; } // Necesario para el siguiente paso (Sectores)
        public string Name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; } // Para que sepan CUÁNDO es
        public string Venue { get; set; } = string.Empty; // Para que sepan DÓNDE es


    }
}
