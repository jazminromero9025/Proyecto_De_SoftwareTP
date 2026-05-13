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

        [HttpGet("{eventId}/sectors")]
        public async Task<ActionResult<List<SectorDto>>> GetByEvent(int eventId)
        {
            var sectors = await _sectorService.GetSectorsByEventAsync(eventId);

            if (sectors == null || !sectors.Any())
                return NotFound(new { Message = $"No se encontraron sectores para el evento con ID {eventId}" });

            return Ok(sectors);
        }

        [HttpPost("{eventId}/sectors")]
        public async Task<ActionResult<SectorDto>> CreateSector(int eventId, [FromBody] CreateSectorRequest request)
        {
            try
            {
                var sector = await _sectorService.CreateSectorAsync(eventId, request.Name, request.Price, request.Capacity);
                return CreatedAtAction(nameof(GetByEvent), new { eventId }, sector);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CreateSectorRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
    }
}
