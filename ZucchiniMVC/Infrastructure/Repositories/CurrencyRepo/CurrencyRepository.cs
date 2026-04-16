using Microsoft.Extensions.Caching.Memory;
using Zucchinimvc.Infrastructure.ApiClients.CurrencyClient;
using Zucchinimvc.Models.DTOs.CurrencyDTOs;
using Zucchinimvc.Services;


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
            
            return await _currencyClient.GetAsync<CurrencyRateResponse>($"latest?base={baseCurrency.ToUpper()}");
        }
    }
}
