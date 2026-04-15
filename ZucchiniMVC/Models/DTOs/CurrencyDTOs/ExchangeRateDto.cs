namespace Zucchinimvc.Models.DTOs.CurrencyDTOs
{
    public class ExchangeRateDto
    {
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
