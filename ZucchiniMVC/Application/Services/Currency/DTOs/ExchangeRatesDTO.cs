namespace Zucchinimvc.Application.Services.Currency.DTOs
{
    public class ExchangeRatesDTO
    {
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
