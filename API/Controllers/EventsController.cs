using Application.Interfaces;
using Application.Models;
using Application.UseCases.Events.Commands;
using Application.UseCases.Events.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/v1/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        // Inyectamos el repositorio por constructor
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }



        [HttpGet]
        // 1. Usamos EventDto para no exponer la entidad de base de datos
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] GetEventsQuery request)
        {
            // 2. La API le pide al Service los datos (pasando el query por si hay filtros)
            var events = await _eventService.GetEventsAsync(request);

            // Si la lista está vacía, igual devolvemos un 200 OK con la lista vacía []
            return Ok(events);
        }


        [HttpPost]
        //[Authorize(Roles = "Administrador")] esto usariamos si instalamos identity
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventCommand command)
        {
            // Llamamos al service para que haga la magia
            var newEvent = await _eventService.CreateEventAsync(command);

            
            // . CreatedAtAction hace 3 cosas:
            //    a) Pone el código de estado en 201.
            //    b) Genera la URL usando el nombre de tu método GET (GetEvents).
            //    c) Mete el objeto 'newEvent' (el DTO) en el cuerpo de la respuesta para que lo veas.
            return CreatedAtAction(nameof(GetEvents), new { id = newEvent.Id }, newEvent);
        }




    }
}
