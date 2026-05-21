using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Analytics;
using Zucchinimvc.Application.Services.UsersService;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers;

public class AdminController : Controller
{
    private readonly IUserService _usersService;
    private readonly IAnalyticsService _analyticsService;

    public AdminController(IUserService usersService, IAnalyticsService analyticsService)
    {
        _usersService = usersService;
        _analyticsService = analyticsService;
    }
    public async Task SetupAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string roleName = "Admin";
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var user = await userManager.FindByEmailAsync("ZucchiniNews@gmail.com");

        if (user != null)
        {
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            Analytics = await _analyticsService.GetDashboardSummaryAsync(
                DateTime.UtcNow.AddDays(-30), DateTime.UtcNow),
            Users = await _usersService.GetAllUsersAsync()
        };
        return View(model);
    }
}
