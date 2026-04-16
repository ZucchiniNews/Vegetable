namespace Zucchinimvc.Infrastructure.Config
{
    public class CurrencySettings
    {
        public string APIKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.currencyfreaks.com/v2.0/";
    }
}
