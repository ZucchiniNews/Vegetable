namespace Zucchinimvc.Infrastructure.Config
{
    public class WeeklyNewsLetterQueueSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
    }
}
