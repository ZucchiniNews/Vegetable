using Zucchinimvc.Models.DTOs.Analytic;

namespace ZucchiniMVC.Infrastructure.ApiClients.AzureInsightClient
{
    public interface IAzureInsightClient
    {
        Task TrackEventAsync(AnalyticsEventDto analyticsEvent);
    }
}