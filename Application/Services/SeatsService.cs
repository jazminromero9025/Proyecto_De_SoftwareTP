using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Interfaces;
using Application.UseCases.Seats.Queries;
using Domain.Entities;

namespace Application.Services
{
    public class SeatsService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        // se inyecta el repository
        public SeatsService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<List<SeatDto>> GetSeatsBySectorAsync(Guid sectorId)
        {
            // El Service instancia el Query 
            var query = new GetSeatsBySectorQuery(sectorId);

            // El Service le pasa el Query al Repositorio
            var seats = await _seatRepository.GetSeatsBySectorQueryAsync(query);

            // Mapeo de entidad a DTO
            return seats.Select(s => new SeatDto
            {
                Id = s.Id,
                Number = s.SeatNumber,
                Status = s.Status.ToString()
            }).ToList();
        }





    }
}
