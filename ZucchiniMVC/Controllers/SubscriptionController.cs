using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Subscriptions;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    public async Task<IActionResult> Index()
    {
        var subscriptionTypes = await _subscriptionService.GetAllSubscriptionTypesAsync();
        return View(subscriptionTypes);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(int subscriptionTypeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }
        var subscription = await _subscriptionService.CreateSubscriptionAsync(userId, subscriptionTypeId);
        return Ok(subscription.Id);
    }
}

