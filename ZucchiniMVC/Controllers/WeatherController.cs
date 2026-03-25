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

            return View(weather);
        }


    }
}
