using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Models.ViewModels;
using Zucchinimvc.Services.API;

namespace Zucchinimvc.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IWeatherService _service;

        public AnalyticsController(IWeatherService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string city)
        {
            city = string.IsNullOrWhiteSpace(city) ? "Linköping" : city;
            var weather = await _service.GetWeatherByCityAsync(city);

            if (weather == null) return View("Error");

            var chartCities = new[] { "Linköping", "Stockholm", "Oslo", "Helsinki", "Copenhagen" };
            weather.Cities = new List<CityWeatherChart>();

            foreach (var cityName in chartCities)
            {
                var history = await _service.GetHistoryAsync(cityName);
                var orderedHistory = history.OrderBy(x => x.RecordedAt).ToList();

                weather.Cities.Add(new CityWeatherChart
                {
                    City = cityName,
                    Labels = orderedHistory.Select(x => x.RecordedAt.ToString("s")).ToList(),
                    Temperatures = orderedHistory.Select(x => x.Temperature).ToList()
                });
            }

            return View(weather);
        }


    }
}
