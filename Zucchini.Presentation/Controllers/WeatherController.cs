using Microsoft.AspNetCore.Mvc;
using Application.Services.Weather;

namespace Presentation.Controllers;

public class WeatherController : Controller
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;

    }
    public async Task<ActionResult> Index(string city)
    {
        var model = await _weatherService.GetWeatherAnalyticsAsync(city);

        if (model == null) return View("Error");

        return View(model);
    }
}
