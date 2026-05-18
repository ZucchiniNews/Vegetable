using Zucchinimvc.Application.Services.Analytics.DTOs;

namespace Zucchinimvc.Infrastructure.ApiClients.AzureInsightClient
{
    public interface IAzureInsightClient
    {
        Task TrackEventAsync(AnalyticsEventDto analyticsEvent);
    }
}