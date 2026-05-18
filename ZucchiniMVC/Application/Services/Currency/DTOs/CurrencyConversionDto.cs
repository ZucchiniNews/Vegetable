namespace Zucchinimvc.Application.Services.Currency.DTOs
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
