namespace Zucchinimvc.Application.Services.Currency
{
    public interface ICurrencyService
    {
        Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency);
    }
}
