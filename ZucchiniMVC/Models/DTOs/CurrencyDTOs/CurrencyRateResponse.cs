namespace Zucchinimvc.Models.DTOs.CurrencyDTOs
{
    public class CurrencyRateResponse
    {
        public string? Base { get; set; }
        public Dictionary<string, decimal>? Rates { get; set; }
        public string? Date { get; set; }
    }
}
