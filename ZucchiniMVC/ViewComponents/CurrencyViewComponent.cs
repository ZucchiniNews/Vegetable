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
            var viewModel = await _currencyService.GetCurrencyWidgetDataAsync("USD");
            return View(viewModel);
        }
    }
}