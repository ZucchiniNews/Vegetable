using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Currency
{
    public interface ICurrencyService
    {
        Task<CurrencyWidgetViewModel> GetCurrencyWidgetDataAsync(string baseCurrency);
        Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency);
    }
}