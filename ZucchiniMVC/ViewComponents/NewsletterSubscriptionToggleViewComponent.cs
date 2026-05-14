using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.UsersService;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.ViewComponents;

public class NewsletterSubscriptionToggleViewComponent : ViewComponent
{
    private readonly IUserService _userService;

    public NewsletterSubscriptionToggleViewComponent(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return View(new NewsletterSubscriptionToggleViewModel());
        }

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return View(new NewsletterSubscriptionToggleViewModel());
        }

        return View(new NewsletterSubscriptionToggleViewModel
        {
            IsSubscribed = user.NewsletterSubscribed,
            Email = user.Email ?? string.Empty
        });
    }
}
