using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/sectors")]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [HttpGet("{sectorId}/seats")]
    public async Task<IActionResult> GetBySector(int sectorId)
    {
        var result = await _seatService.GetSeatsBySectorAsync(sectorId);

        // VALIDACIÓN CLAVE:
        // Si el resultado es null o la lista está vacía, 
        // asumimos que el sector no existe
        if (result == null || !result.Any())
        {
            
            return NotFound($"No se encontró el sector con ID {sectorId}");
        }

        // Si llegamos acá, es porque hay datos
        return Ok(result);
    }
}