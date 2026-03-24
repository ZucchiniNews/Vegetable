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
            if (string.IsNullOrEmpty(city))
            {
                return View();
            }

            var weather = await _service.GetWeatherByCityAsync(city);

            if (weather == null)
            {
                return View("Error");
            }
            return View(weather);
        }


    }
}
