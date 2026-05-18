using System.Text.Json.Serialization;

namespace Zucchinimvc.Application.Services.Currency.DTOs
{
    public class CurrencyRateResponse
    {

        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("base")]
        public string? Base { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, string>? Rates { get; set; }

    }
}
