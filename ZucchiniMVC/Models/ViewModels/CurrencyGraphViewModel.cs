namespace Zucchinimvc.Models.ViewModels
{
    public class CurrencyGraphViewModel
    {
       

        public string BaseCurrency { get; set; } = string.Empty;
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public string Range { get; internal set; }
    }
}
