using System.Text.Json.Serialization;

namespace Zucchinimvc.Models.DTOs.CurrencyDTOs;

public class CurrencyRateResponse
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; }

    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; set; }
}
