using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sectors
{
    public class GetSectorsByEventQuery : IGetSectorsByEventQuery
    {
        public async Task<List<SectorDto>> Execute(ISectorRepository repository, int eventId)
        {
            var sectors = await repository.GetByEventIdAsync(eventId);

            return sectors.Select(s => new SectorDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                EventId = s.EventId
            }).ToList();
        }

    }
}
