using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Currency;

namespace Zucchinimvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ICurrencyService currencyService, ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService;
            _logger = logger;
        }

        [HttpGet("{baseCurrency}")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetRates(string baseCurrency = "USD")
        {
            // Quick guard clause for the request itself
            if (string.IsNullOrWhiteSpace(baseCurrency))
                return BadRequest("Base currency is required.");

            try
            {
                var rates = await _currencyService.GetRatesAsync(baseCurrency);
                return Ok(rates);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("usd")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetUSDRates()
        {
            var rates = await _currencyService.GetLatestRatesAsync("USD");

            if (rates == null || rates.Count == 0)
            {
                return NotFound("Unable to fetch USD rates");
            }

            return Ok(rates);
        }

        [HttpGet("eur")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetEURates()
        {
            var rates = await _currencyService.GetLatestRatesAsync("EUR");

            if (rates == null || rates.Count == 0)
            {
                return NotFound("Unable to fetch EUR rates");
            }

            return Ok(rates);
        }

        [HttpGet("sek")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetSEKRates()
        {
            var rates = await _currencyService.GetLatestRatesAsync("SEK");

            if (rates == null || rates.Count == 0)
            {
                return NotFound("Unable to fetch SEK rates");
            }

            return Ok(rates);
        }
    }
}
