using Zucchinimvc.Models.DTOs.Analytic;

namespace Zucchinimvc.Infrastructure.ApiClients.AzureInsightClient
{
    public interface IAzureInsightClient
    {
        Task TrackEventAsync(AnalyticsEventDto analyticsEvent);
    }
}