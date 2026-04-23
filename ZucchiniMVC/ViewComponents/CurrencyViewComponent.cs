using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Currency;

namespace Zucchinimvc.ViewComponents
{
    public class CurrencyViewComponent : ViewComponent
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyViewComponent(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usdRates = await _currencyService.GetLatestRatesAsync("USD");

            // Debug: Check if we got data
            if (usdRates == null)
            {
                return Content("DEBUG: usdRates is NULL");
            }

            if (usdRates.Count == 0)
            {
                return Content("DEBUG: usdRates has 0 items");
            }

            var sekEurRates = new Dictionary<string, decimal>();

            if (usdRates.ContainsKey("SEK"))
                sekEurRates.Add("SEK", usdRates["SEK"]);

            if (usdRates.ContainsKey("EUR"))
                sekEurRates.Add("EUR", usdRates["EUR"]);

            // Debug: Show what we found
            if (sekEurRates.Count == 0)
            {
                return Content($"DEBUG: No SEK/EUR found. Available currencies: {string.Join(", ", usdRates.Keys.Take(10))}");
            }

            return View(sekEurRates);
        }
    }
}