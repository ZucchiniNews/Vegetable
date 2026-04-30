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

        // GET: api/currency/rates/USD
        [HttpGet("rates/{baseCurrency}")]
        public async Task<IActionResult> GetRates(string baseCurrency)
        {
            if (string.IsNullOrWhiteSpace(baseCurrency))
                return BadRequest("Base currency is required.");

            var rates = await _currencyService.GetLatestRatesAsync(baseCurrency.ToUpper());

            if (rates == null || !rates.Any())
            {
                return NotFound($"No rates found for {baseCurrency}");
            }

            return Ok(rates);
        }

        // GET: api/currency/eur
        [HttpGet("eur")]
        public async Task<IActionResult> GetEurRates()
        {
            // The Controller decides to ask the service for "EUR"
            var rates = await _currencyService.GetLatestRatesAsync("EUR");

            if (!rates.Any()) return NotFound("EUR rates unavailable.");

            return Ok(rates);
        }

        // GET: api/currency/sek
        [HttpGet("sek")]
        public async Task<IActionResult> GetSekRates()
        {
            var rates = await _currencyService.GetLatestRatesAsync("SEK");

            if (!rates.Any()) return NotFound("SEK rates unavailable.");

            return Ok(rates);
        }

        // GET: api/currency/widget/USD
        [HttpGet("widget/{baseCurrency}")]
        public async Task<IActionResult> GetWidgetData(string baseCurrency = "USD")
        {
            var viewModel = await _currencyService.GetCurrencyWidgetDataAsync(baseCurrency);

            if (viewModel.HasError)
                return StatusCode(500, "Error fetching widget data");

            return Ok(viewModel);
        }
    }
}