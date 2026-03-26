using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Services.API;

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

            TimeZoneInfo tz;
            try
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm");
            }
            catch
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            }

            var orderedHistory = history.OrderBy(x => x.RecordedAt).ToList();

            weather.Labels = orderedHistory
    .Select(x => TimeZoneInfo.ConvertTimeFromUtc(x.RecordedAt, tz)
        .ToString("yyyy-MM-ddTHH:mm:ss"))
    .ToList();

            weather.Temperatures = orderedHistory
                .Select(x => x.Temperature)
                .ToList();

            return View(weather);
        }


    }
}
