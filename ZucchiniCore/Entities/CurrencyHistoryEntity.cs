namespace ZucchiniCore.Entities
{
    public class CurrencyHistoryEntity
    {
        public int Id { get; set; }

        public string BaseCurrency { get; set; } = "USD";

        public string TargetCurrency { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public decimal Rate { get; set; }
    }
}
