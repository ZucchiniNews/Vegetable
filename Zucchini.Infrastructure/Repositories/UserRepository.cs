using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Roles> _roleManager;

    public UserRepository(UserManager<User> userManager, RoleManager<Roles> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<User?> GetByIdAsync(string userId)
        => await _userManager.FindByIdAsync(userId);

    public async Task<User?> GetByEmailAsync(string email)
        => await _userManager.FindByEmailAsync(email);

    public async Task<User?> GetWithSubscriptionAsync(string userId)
        => await _userManager.Users
            .Include(u => u.Subscriptions)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<List<User>> GetAllAsync()
        => await _userManager.Users.ToListAsync();

    public async Task<List<User>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _userManager.Users
            .Where(u =>
                u.Email!.ToLower().Contains(term) ||
                u.FirstName.ToLower().Contains(term) ||
                u.LastName.ToLower().Contains(term))
            .ToListAsync();
    }

    public async Task<List<User>> GetNewsletterSubscribersAsync()
        => await _userManager.Users
            .Where(u => u.NewsletterSubscribed)
            .ToListAsync();

    public async Task UpdateAsync(User user)
        => await _userManager.UpdateAsync(user);

    public async Task<IList<string>> GetRolesAsync(User user)
        => await _userManager.GetRolesAsync(user);

    public async Task<bool> RoleExistsAsync(string roleName)
        => await _roleManager.RoleExistsAsync(roleName);

    public async Task AddToRoleAsync(User user, string roleName)
        => await _userManager.AddToRoleAsync(user, roleName);

    public async Task RemoveFromRoleAsync(User user, string roleName)
        => await _userManager.RemoveFromRoleAsync(user, roleName);

    public async Task LockAsync(User user)
    {
        await _userManager.SetLockoutEnabledAsync(user, true);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
    }

    public async Task UnlockAsync(User user)
        => await _userManager.SetLockoutEndDateAsync(user, null);
}