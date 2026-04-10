using Microsoft.AspNetCore.Mvc;
using Application.Services.Weather;
using Presentation.Models.ViewModels;

namespace Presentation.ViewComponents;

public class WeatherViewComponent : ViewComponent
{
    private readonly IWeatherService _service;

    public WeatherViewComponent(IWeatherService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync(string city)
    {
        city = string.IsNullOrWhiteSpace(city) ? "Linköping" : city;

        var weather = await _service.GetWeatherByCityAsync(city);

        var model = weather != null
            ? new WeatherViewModel
            {
                City = city,
                Temp = weather.Temperature,
                Icon = weather.Icon
            }
            : new WeatherViewModel { City = city };

        return View(model);
    }
}