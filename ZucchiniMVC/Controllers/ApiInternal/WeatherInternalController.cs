using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Controllers.ApiInternal.Filters;

namespace Zucchinimvc.Controllers.ApiInternal;

[ApiController]
[Route("api/internal/weather")]
[ApiKeyAuth]
public class WeatherInternalController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherInternalController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpPost("save-history")]
    public async Task<IActionResult> SaveHistory([FromBody] string city)
    {
        var weather = await _weatherService.GetWeatherByCityAsync(city);
        if (weather == null) return NotFound();

        await _weatherService.SaveWeatherHistoryAsync(weather);
        return Ok();
    }
}