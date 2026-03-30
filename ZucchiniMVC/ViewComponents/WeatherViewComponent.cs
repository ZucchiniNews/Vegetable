using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.ViewComponents;

public class WeatherViewComponent : ViewComponent
{
    private readonly WeatherService _service;

    public WeatherViewComponent(WeatherService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync(string city)
    {
        city = string.IsNullOrWhiteSpace(city) ? "Linköping" : city;

        var weather = await _service.GetWeatherByCityAsync(city);

        return View(weather ?? new WeatherViewModel { City = city });
    }
}