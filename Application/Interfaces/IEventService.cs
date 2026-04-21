using Application.Models;
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

    }
}
