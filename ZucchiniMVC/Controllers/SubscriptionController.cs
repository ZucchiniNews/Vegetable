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
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(int subscriptionTypeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var subscription = await _subscriptionService
            .CreateSubscriptionAsync(userId, subscriptionTypeId);
        return Ok(subscription.Id);
    }
}

