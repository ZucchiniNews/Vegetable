using System.Globalization;
using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;

namespace Zucchinimvc.Application.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly ICurrencyRepository _currencyRepo;

        public CurrencyService(ILogger<CurrencyService> logger, ICurrencyRepository currencyRepo)
        {
            _logger = logger;
            _currencyRepo = currencyRepo;
        }

        // Service returns DTO, not ViewModel


        public async Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency)
        {
            var response = await _currencyRepo.GetLatestRatesAsync(baseCurrency);

            if (response?.Rates == null)
            {
                _logger.LogWarning("Repository returned null rates for {Base}", baseCurrency);
                return new Dictionary<string, decimal>();
            }

            var results = new Dictionary<string, decimal>();

            foreach (var rate in response.Rates)
            {
                if (decimal.TryParse(rate.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var val))
                {
                    results.Add(rate.Key, val);
                }
            }

            return results;
        }
    }
}