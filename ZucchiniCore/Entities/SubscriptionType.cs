namespace ZucchiniCore.Entities
{
    public class SubscriptionType
    {
        public int Id { get; set; }

        public string TypeName { get; set; } = "None";
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
