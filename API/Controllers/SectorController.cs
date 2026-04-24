using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Application.Services;


namespace API.Controllers
{

    [ApiController]
    [Route("api/v1/events")]
    public class SectorController : ControllerBase
    {

        private readonly ISectorService _sectorService;

        // Inyectamos el servicio de sectores
        public SectorController(ISectorService sectorService)
        {
            _sectorService = sectorService;
        }

        [HttpGet("{eventId}/sectors")] //("event/{eventId}")
        public async Task<ActionResult<List<SectorDto>>> GetByEvent(int eventId)
        {
            var sectors = await _sectorService.GetSectorsByEventAsync(eventId);

            if (sectors == null || !sectors.Any())
            {
                return NotFound(new { Message = $"No se encontraron sectores para el evento con ID {eventId}" });
            }

            return Ok(sectors);
        }


    }
}
