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

        if (result == null || !result.Any())
            return NotFound(new { message = $"No se encontraron butacas para el sector {sectorId}." });

        return Ok(result);
    }
}