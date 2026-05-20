using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Currency.DTOs;

namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public interface ICurrencyRepository
    {
        Task<CurrencyRateResponse?> GetLatestRatesAsync(string baseCurrency);

        Task<List<CurrencyHistoryEntity>> GetCurrencyHistoryAsync(
            string baseCurrency,
            string targetCurrency,
            DateTime fromDate);
    }
}