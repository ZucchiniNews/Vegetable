using Zucchinimvc.Models.DTOs.CurrencyDTOs;

namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public class ICurrencyRepository
    {
        Task<CurrencyRateResponse?> GetLatestRatesAsync(string baseCurrency);

    }
}
