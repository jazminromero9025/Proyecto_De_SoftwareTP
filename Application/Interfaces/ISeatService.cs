using Application.DTOs;
using Application.UseCases.Seats.Queries;

namespace Application.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatDTO>> GetSeatsBySectorAsync(int sectorId);
    }
}