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

        //ID DEL EVENTO
        public int EventId { get; set; } // FK hacia Evento

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        // Relación: Un sector "contiene" muchas butacas
        public List<Seat> Seats { get; set; } = [];

        // Esto le permite a Entity Framework "dibujar" la línea hacia EVENT
        public virtual Event Event { get; set; } = null!;


    }
}
