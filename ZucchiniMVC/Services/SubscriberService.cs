using Zucchinimvc.Repositories;

namespace Zucchinimvc.Services;

public class SubsscriberService : ISubscriberService
{
    Task<Subscription?> GetSubscriptionByUserIdAsync(string userId)
    {

    }

    Task<IEnumerable<SubscriptionType>> GetAllSubscriptionTypesAsync()
    {

    }
    Task SubscribeAsync(string userId, int subscriptionTypeId)
    {

    }
    Task UnsubscribeAsync(string userId)
    {
        
    }
}