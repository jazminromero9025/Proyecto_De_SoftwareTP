using Application.UseCases.Seats.Queries;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISeatRepository
    {
        // El repositorio recibe el objeto Query completo
        Task<IEnumerable<Seat>> GetSeatsBySectorQueryAsync(GetSeatsBySectorQuery query);

    }
}
