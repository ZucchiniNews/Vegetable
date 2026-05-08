using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Currency;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.ViewComponents;

public class CurrencyViewComponent : ViewComponent
{
    private readonly ICurrencyService _service;

    public CurrencyViewComponent(ICurrencyService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync(string baseCurrency = "USD")
    {
        var currencyData = await _service.GetLatestRatesAsync(baseCurrency);

        var viewModel = new CurrencyWidgetViewModel
        {
            Rates = currencyData,
            HasError = currencyData == null || !currencyData.Any(),
            LastUpdated = DateTime.UtcNow
        };

        return View(viewModel);
    }
}