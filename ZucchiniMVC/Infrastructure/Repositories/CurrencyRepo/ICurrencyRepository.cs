using Zucchinimvc.Models.DTOs.CurrencyDTOs;

namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public interface ICurrencyRepository
    {
        Task<CurrencyRateResponse?> GetLatestRatesAsync(string baseCurrency);
    }
}
