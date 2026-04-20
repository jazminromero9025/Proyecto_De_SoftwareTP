using Application.UseCases.Seats.Queries;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Interfaces
{
    public class ISeatRepository
    {
        // El repositorio recibe el objeto Query completo
        Task<IEnumerable<Seat>> GetSeatsBySectorQueryAsnyc(GetSeatsBySectorQuery query);

    }
}
