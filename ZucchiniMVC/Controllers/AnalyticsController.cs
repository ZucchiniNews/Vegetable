using ZucchiniCore.Entities;
using Zucchinimvc.Models.DTOs.Analytic;
using Zucchinimvc.Application.Services.Analytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZucchiniCore.enums;

namespace Zucchinimvc.Controllers;

[Authorize(Roles = "Admin")]
public class AnalyticsController : Controller
{
    private readonly IAnalyticsService _analyticsService;
    private readonly UserManager<User> _userManager;

    public AnalyticsController(IAnalyticsService analyticsService, UserManager<User> userManager)
    {
        _analyticsService = analyticsService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var summary = await _analyticsService.GetDashboardSummaryAsync(
            DateTime.UtcNow.AddDays(-30),
            DateTime.UtcNow
        );

        return View(summary);
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TrackEvent(string eventType, string resourceId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user?.Id;

        if (Enum.TryParse<EventType>(eventType, out var parsedEventType))
        {
            await _analyticsService.TrackAsync(parsedEventType, resourceId, userId);
            return Ok();
        }
        else
        {
            return BadRequest("Invalid event type");
        }
    }
}