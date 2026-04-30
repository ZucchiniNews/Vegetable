using Zucchinimvc.Models.DTOs.CurrencyDTOs;

namespace Zucchinimvc.Infrastructure.ApiClients.CurrencyClient
{
    public class ICurrencyApiClient
    {
        Task<CurrencyRateDto> GetLatestRatesAsync();
        Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
        Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency);
    }
}
