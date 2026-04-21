using Microsoft.AspNetCore.Mvc;
using Application.Interfaces; 
using Domain.Entities; 


namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        // Inyectamos el repositorio por constructor
        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            // Llamamos al método que vamos a crear en el repositorio
            var events = await _eventRepository.GetAllEventsAsync();

            // Si la lista está vacía, igual devolvemos un 200 OK con la lista vacía []
            return Ok(events);
        }





    }
}
