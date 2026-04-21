using Application.UseCases.Seats.Queries;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISeatRepository
    {
        // El repositorio recibe el objeto Query completo
        Task<IEnumerable<Seat>> GetSeatsBySectorQueryAsync(GetSeatsBySectorQuery query);
    }
}