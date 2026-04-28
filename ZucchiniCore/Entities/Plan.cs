namespace ZucchiniCore.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public required string ProviderPriceId { get; set; }
    }
}
