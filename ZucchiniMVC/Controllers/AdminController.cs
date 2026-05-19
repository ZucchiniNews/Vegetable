using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.UsersService;

namespace Zucchinimvc.Controllers;

public class AdminController : Controller
{
    private readonly IUserService _usersService;

    public AdminController(IUserService usersService)
    {
        _usersService = usersService;
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
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _usersService.GetAllUsersAsync();
        return View(users);
    }
}
