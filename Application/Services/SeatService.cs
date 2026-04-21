using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Seats.Queries;
using Domain.Entities;

namespace Application.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        // se inyecta el repository
        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<List<SeatDTO>> GetSeatsBySectorAsync(int sectorId)
        {
            // El Service crea el Query 
            var query = new GetSeatsBySectorQuery(sectorId);

            // El Service le pasa el Query al Repositorio
            var seats = await _seatRepository.GetSeatsBySectorQueryAsync(query);

            // Mapeo de entidad a DTO
            return seats.Select(s => new SeatDTO
            {
                Id = s.Id,
                Number = s.SeatNumber,
                Status = s.Status.ToString()
            }).ToList();
        }
    }
}