using Zucchinimvc.Application.Services.Currency.DTOs;

namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public interface ICurrencyRepository
    {
        Task<CurrencyRateResponse?> GetLatestRatesAsync(string baseCurrency);
    }
}
