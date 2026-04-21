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
        // 1. El Controller recibe el dato (sectorId)
        // 2. Simplemente le dice al Service: "Tomá, procesá esto"
        var result = await _seatService.GetSeatsBySectorAsync(sectorId);

        return Ok(result);
    }
}