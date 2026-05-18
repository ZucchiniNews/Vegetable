namespace Zucchinimvc.Application.Services.Currency.DTOs
{
    public class CurrencyWidgetDto
    {
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public bool HasError { get; set; }
        public string BaseCurrency { get; set; } = string.Empty;
    }
}