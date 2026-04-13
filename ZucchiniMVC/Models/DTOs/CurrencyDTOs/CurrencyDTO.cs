namespace Zucchinimvc.Models.DTOs.CurrencyDTOs
{
    public class CurrencyDTO
    {
        public Dictionary<string, decimal> Rates { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public bool Success { get; set; }
    }
}
