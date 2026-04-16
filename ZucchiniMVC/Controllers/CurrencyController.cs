using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Currency;

namespace Zucchinimvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
           
        }

        //[HttpGet("{baseCurrency}")]
        //public async Task<ActionResult<Dictionary<string, decimal>>> GetRates(string baseCurrency = "USD")
        //{
        //    // Quick guard clause for the request itself
        //    if (string.IsNullOrWhiteSpace(baseCurrency))
        //        return BadRequest("Base currency is required.");

        //    try
        //    {
        //        var rates = await _currencyService.GetRatesAsync(baseCurrency);
        //        return Ok(rates);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        _logger.LogWarning(ex.Message);
        //        return NotFound(ex.Message);
        //    }
        //}

        
    }
}
