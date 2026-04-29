namespace Zucchinimvc.Models.ViewModels
{
    public class CurrencyWidgetViewModel
    {
        public Dictionary<string, decimal>? Rates { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
