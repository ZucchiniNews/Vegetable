namespace Zucchinimvc.Models.ViewModels
{
    public class CurrencyRateViewModel
    {
        public string BaseCurrency { get; set; } = string.Empty;
        public Dictionary<string, decimal>? Rates { get; set; }
        public string LastUpdated { get; set; } = string.Empty;
    }
}
