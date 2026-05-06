using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.ViewComponents;

public class SubscriptionStatusViewComponent : ViewComponent
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IPlanService _planService;

    public SubscriptionStatusViewComponent(
        ISubscriptionService subscriptionService,
        IPlanService planService)
    {
        _subscriptionService = subscriptionService;
        _planService = planService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return View(new SubscriptionStatusViewModel());
        }

        var subscription = await _subscriptionService
            .GetLatestSubscriptionForUserAsync(userId);

        if (subscription == null)
        {
            return View(new SubscriptionStatusViewModel());
        }

        var planName = "Unknown Plan";

        if (!string.IsNullOrEmpty(subscription.PlanId) &&
            int.TryParse(subscription.PlanId, out var planId))
        {
            var plan = await _planService.FindPlanByIdAsync(planId);
            planName = plan?.Name ?? planName;
        }

        return View(new SubscriptionStatusViewModel
        {
            PlanName = planName,
            Status = subscription.Status,
            ActivatedAt = subscription.ActivatedAt
        });
    }
}