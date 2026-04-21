using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.UseCases.Events.Queries
{
    
        // Implementamos la interfaz. 
        // Los paréntesis vacíos significan que no pasamos parámetros por ahora.
        public record GetEventsQuery() : IGetEventsQuery;
    
}
