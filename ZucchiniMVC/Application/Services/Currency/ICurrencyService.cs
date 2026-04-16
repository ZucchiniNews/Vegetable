using Zucchinimvc.Models.DTOs.CurrencyDTOs;
using Zucchinimvc.Models.ViewModels;
namespace Zucchinimvc.Application.Services.Currency
{
    public interface ICurrencyService
    {
        Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency);
    }
}
