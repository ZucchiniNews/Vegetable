using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Currency;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Currency()
        {
            var baseCurrency = "USD";

            var rates = await _currencyService.GetLatestRatesAsync(baseCurrency);

            var model = new CurrencyGraphViewModel
            {
                BaseCurrency = baseCurrency,
                Rates = rates
            };

            return View(model);
        }
    }
}