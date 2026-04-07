using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Weather;



namespace Zucchinimvc.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _analyticsService;

        public WeatherController(IWeatherService analyticsService)
        {
            _analyticsService = analyticsService;

        }
        public async Task<ActionResult> Index(string city)
        {
            var model = await _analyticsService.GetWeatherAnalyticsAsync(city);

            if (model == null) return View("Error");

            return View(model);
        }


    }
}
