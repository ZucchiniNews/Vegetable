using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Analytics;



namespace Zucchinimvc.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
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
