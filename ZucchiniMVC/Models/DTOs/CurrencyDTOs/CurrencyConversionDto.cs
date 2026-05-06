namespace Zucchinimvc.Models.DTOs.CurrencyDTOs
{
    public class CurrencyConversionDto
    {
        public decimal Amount { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal Rate { get; set; }
    }
}
