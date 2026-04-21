using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        // Inyectamos el repositorio para poder hablar con la base de datos
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }



        public async Task<IEnumerable<EventDto>> GetEventsAsync(IGetEventsQuery query)
        {
            var events = await _eventRepository.GetAllEventsAsync();

            return events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                EventDate = e.EventDate,
                Venue = e.Venue
            }).ToList();
        }





    }
}
