using Zucchinimvc.Models.DTOs.CurrencyDTOs;

namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public class ICurrencyRepository
    {
        Task<CurrencyRateDto> GetLatestRatesAsync();
        Task<ExchangeRateDto> GetExchangeRatesAsync(string currencyCode);
        Task UpdateRatesCacheAsync();
        Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
    }
}
