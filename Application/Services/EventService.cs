using Application.Interfaces;
using Application.Models;
using Application.UseCases.Events.Commands;
using Domain.Entities;
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
            // 1. Buscamos las entidades en la base de datos
            var events = await _eventRepository.GetAllEventsAsync();
            // 2. Mapeamos las Entities (Domain) a DTOs (Application)
            return events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                EventDate = e.EventDate,
                Venue = e.Venue
            }).ToList();
        }

        public async Task<EventDto> CreateEventAsync(CreateEventCommand command)
        {
            // 1. Creamos la Entidad de Dominio con los datos del Command
            var eventEntity = new Event
            {
                Name = command.Name,
                EventDate = command.EventDate,
                Venue = command.Venue
            };

            // 2. Le pedimos al repositorio que lo guarde
            await _eventRepository.AddAsync(eventEntity);

            // PASO 3: El Mapeo de Salida (Entity -> DTO)
            // Una vez guardado, la base de datos le asignó un ID a 'eventEntity'.
            // Ahora convertimos esa entidad en un DTO para devolverlo al Controller.
            // 3. Devolvemos el DTO (para que la API no muestre la entidad cruda)
            return new EventDto
            {
                Id = eventEntity.Id,
                Name = eventEntity.Name,
                EventDate = eventEntity.EventDate,
                Venue = eventEntity.Venue
            };
        }



    }
}
