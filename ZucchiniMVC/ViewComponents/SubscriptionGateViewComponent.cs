using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Models.ViewModels;



namespace Zucchinimvc.ViewComponents
{
    public class SubscriptionGateViewComponent : ViewComponent
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionGateViewComponent(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string body, bool isAdmin)
        {
            var userId = UserClaimsPrincipal.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            bool isSubscribed = false;

            if (!string.IsNullOrEmpty(userId))
            {
                isSubscribed = await _subscriptionService.UserHasActiveSubscription(userId);
            }

            var model = new SubscriptionGateViewModel
            {
                IsSubscribed = isSubscribed || isAdmin,
                Body = body
            };

            return View(model);
        }
    }
}
