using Application.Interfaces;
using Application.Models;
using Application.UseCases.Sectors;
using Domain.Entities;

namespace Application.Services
{
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _sectorRepository;

        public SectorService(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }



        public async Task<List<SectorDto>> GetSectorsByEventAsync(int eventId)
        {
            IGetSectorsByEventQuery query = new GetSectorsByEventQuery();
            return await query.Execute(_sectorRepository, eventId);
        }

        public async Task<SectorDto> CreateSectorAsync(int eventId, string name, decimal price, int capacity)
        {
            var sector = new Sector
            {
                EventId = eventId,
                Name = name,
                Price = price,
                Capacity = capacity,
                Seats = Enumerable.Range(1, capacity).Select(i => new Seat
                {
                    Id = Guid.NewGuid(),
                    SeatNumber = i,
                    RowIdentifier = "A",
                    Status = "Available",
                    Version = 0
                }).ToList()
            };

            var created = await _sectorRepository.CreateSectorAsync(sector);

            return new SectorDto
            {
                Id = created.Id,
                Name = created.Name,
                Price = created.Price,
                EventId = created.EventId
            };
        }
    }
}
