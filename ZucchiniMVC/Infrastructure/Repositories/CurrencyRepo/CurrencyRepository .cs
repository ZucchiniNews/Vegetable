using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Currency.DTOs;
using Zucchinimvc.Infrastructure.ApiClients.CurrencyClient;
using Zucchinimvc.Infrastructure.Data;

namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyClient _currencyClient;
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(
            CurrencyClient currencyClient,
            ApplicationDbContext context)
        {
            _currencyClient = currencyClient;
            _context = context;
        }

        public async Task<CurrencyRateResponse?> GetLatestRatesAsync(string baseCurrency)
        {
            return await _currencyClient.GetAsync<CurrencyRateResponse>(
                $"rates/latest?base={baseCurrency.ToUpper()}");
        }

        public async Task<List<CurrencyHistoryEntity>> GetCurrencyHistoryAsync(
            string baseCurrency,
            string targetCurrency,
            DateTime fromDate)
        {
            return await _context.CurrencyHistory
                .Where(x => x.BaseCurrency == baseCurrency
                         && x.TargetCurrency == targetCurrency
                         && x.Date >= fromDate)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }
    }
}