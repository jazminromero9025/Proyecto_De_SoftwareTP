using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SeatDTO
    {
        public Guid Id { get; set; }
        public int Number { get; set; }

        // Lo devolvemos como string para que en Swagger diga "Available" en vez de "0"
        public string Status { get; set; } = string.Empty;

        // No incluimos el RowVersion ni el SectorId acá 
        // porque el DTO es solo para mostrar los datos que el usuario necesita ver.
    }
}