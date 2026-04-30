namespace Zucchinimvc.Application.Services.Currency
{
    public class ICurrencyService
    {
        Task<CurrencyWidgetViewModel> GetCurrencyWidgetDataAsync(string baseCurrency);
        Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency);
    }
}
