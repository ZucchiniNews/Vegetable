using Zucchinimvc.Models.DTOs.Analytic;
using Microsoft.ApplicationInsights;

namespace ZucchiniMVC.Infrastructure.ApiClients.AzureInsightClient
{
    public class AzureInsightClient : IAzureInsightClient
    {
        private readonly TelemetryClient _telemetryClient;

        public AzureInsightClient(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        // This method tracks analytics events and allows mocking for unit testing
        public Task TrackEventAsync(AnalyticsEventDto dto)
        {
            var properties = dto.MetaData.ToDictionary(kv => kv.Key, kv => kv.Value);
            properties["ResourceId"] = dto.ResourceId;
            properties["UserId"] = dto.UserId ?? "anonymous";

            _telemetryClient.TrackEvent(dto.EventType.ToString(), properties);

            return Task.CompletedTask;
        }
    }
}