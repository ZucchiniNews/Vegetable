using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Services;

namespace Zucchinimvc.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherService _service;

        public WeatherController(WeatherService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string city)
        {
            city = string.IsNullOrWhiteSpace(city) ? "Linköping" : city;

            var weather = await _service.GetWeatherByCityAsync(city);

            if (weather == null)
            {
                return View("Error");
            }

            var history = await _service.GetHistoryAsync(city);

            weather.Labels = history
                .OrderBy(x => x.RecordedAt)
                .Select(x => x.RecordedAt.ToString("MM-dd HH:mm"))
                .ToList();

            weather.Temperatures = history
                .OrderBy(x => x.RecordedAt)
                .Select(x => x.Temperature)
                .ToList();

            return View(weather);
        }


    }
}
