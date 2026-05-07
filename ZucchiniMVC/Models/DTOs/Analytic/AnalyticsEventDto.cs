using ZucchiniCore.Entities;
namespace Zucchinimvc.Models.DTOs.Analytic;

public class AnalyticsEventDto

{
    public EventType EventType { get; set; }
    public string ResourceId { get; set; } = string.Empty;
    public string? UserId { get; set;}
    public DateTime Timestamp { get; set; }
    public Dictionary<string, string> MetaData { get; set; } = new();
}
