using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Seats.Queries
{
    

    // Esto ya define la clase, el constructor y la propiedad en un solo paso
        public record GetSeatsBySectorQuery(Guid SectorId);

    
}
