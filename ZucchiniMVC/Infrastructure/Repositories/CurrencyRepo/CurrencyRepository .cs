using Zucchinimvc.Application.Services.Currency.DTOs;
using Zucchinimvc.Infrastructure.ApiClients.CurrencyClient;


namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyClient _currencyClient;

        public CurrencyRepository(CurrencyClient currencyClient)
        {
            _currencyClient = currencyClient;
        }

        public async Task<CurrencyRateResponse?> GetLatestRatesAsync(string baseCurrency)
        {

            return await _currencyClient.GetAsync<CurrencyRateResponse>($"rates/latest?base={baseCurrency.ToUpper()}");
        }
    }
}
