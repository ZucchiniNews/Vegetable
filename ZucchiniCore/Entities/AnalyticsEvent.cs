using ZucchiniCore.enums;

namespace ZucchiniCore.Entities;

public class AnalyticsEvent
{
    public int Id { get; set; }
    public EventType EventType { get; set; }
    public string ResourceId { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public DateTime TimeStamp { get; set; }
    public Dictionary<string, string> MetaData { get; set; } = new();
}
