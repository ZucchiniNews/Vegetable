using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepo;

        public CurrencyService(ICurrencyRepository currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public async Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency)
        {
            var response = await _currencyRepo.GetLatestRatesAsync(baseCurrency);

            if (response == null || response.Rates == null)
                return new Dictionary<string, decimal>();

            return response.Rates;
        }

    }

}
