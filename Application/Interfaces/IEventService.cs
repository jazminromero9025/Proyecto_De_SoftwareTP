using Application.Models;
using Application.UseCases.Events.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEventService
    {
        // El método recibe la interfaz del Query y devuelve una colección de DTOs
        Task<IEnumerable<EventDto>> GetEventsAsync(IGetEventsQuery query);
        Task<EventDto> CreateEventAsync(CreateEventCommand command);

    }
}
