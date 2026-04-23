using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Models.DTOs.CurrencyDTOs;
namespace Zucchinimvc.Application.Services.Currency
{
    public interface ICurrencyService
    {
        Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency);
        Task<Dictionary<string, decimal>> GetRatesAsync(string toCurrency);
        Task<ActionResult<Dictionary<string, decimal>>> GetEURRates();
        Task<ActionResult<Dictionary<string, decimal>>> GetSEKRates();
    }
}
