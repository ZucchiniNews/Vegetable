
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Subscriptions;
namespace Zucchinimvc.ViewComponents
{
    public class SubscriptionStatusViewComponent : ViewComponent
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionStatusViewComponent(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isSubscribed = false;

            if (!string.IsNullOrEmpty(userId))
            {
                isSubscribed = await _subscriptionService.UserHasActiveSubscription(userId);
            }

            return View(isSubscribed);
        }
    }

}


