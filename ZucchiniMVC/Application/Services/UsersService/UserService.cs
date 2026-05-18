using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs.NewsLetterSubscriber;
using ZucchiniCore.Entities;
namespace Zucchinimvc.Application.Services.UsersService;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Roles> _roleManager;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<User> userManager,
        RoleManager<Roles> roleManager,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        try
        {
            return await _userManager.FindByIdAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by id {UserId}", userId);
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by email {Email}", email);
            throw;
        }
    }

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user {UserId}", user.Id);
            throw;
        }
    }

    public async Task DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            // Soft delete — disable account without removing data
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user {UserId}", userId);
            throw;
        }
    }

    public async Task AnonymizeUserDataAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            // Replace personal data with anonymized values
            user.FirstName = "Deleted";
            user.LastName = "User";
            user.Email = $"deleted_{userId}@anonymized.com";
            user.UserName = $"deleted_{userId}";
            user.PhoneNumber = null;
            user.DateOfBirth = null;
            user.NewsletterSubscribed = false;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to anonymize user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to anonymize user {UserId}", userId);
            throw;
        }
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            return await _userManager.GetRolesAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get roles for user {UserId}", userId);
            throw;
        }
    }

    public async Task<User?> GetUserWithRolesAsync(string userId)
    {
        try
        {
            return await _userManager.FindByIdAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user with roles {UserId}", userId);
            throw;
        }
    }

    public async Task AssignRoleAsync(string userId, string roleName)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new ArgumentException($"Role {roleName} does not exist.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to assign role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign role {Role} to user {UserId}", roleName, userId);
            throw;
        }
    }

    public async Task RemoveRoleAsync(string userId, string roleName)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to remove role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove role {Role} from user {UserId}", roleName, userId);
            throw;
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            return await _userManager.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all users");
            throw;
        }
    }

    public async Task<List<User>> SearchUsersAsync(string searchTerm)
    {
        try
        {
            var term = searchTerm.ToLower();
            return await _userManager.Users
                .Where(u =>
                    u.Email!.ToLower().Contains(term) ||
                    u.FirstName.ToLower().Contains(term) ||
                    u.LastName.ToLower().Contains(term))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search users with term {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task LockUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to lock user {UserId}", userId);
            throw;
        }
    }

    public async Task UnlockUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unlock user {UserId}", userId);
            throw;
        }
    }

    public async Task<User?> GetUserWithSubscriptionAsync(string userId)
    {
        try
        {
            return await _userManager.Users
                .Include(u => u.Subscriptions)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user with subscription {UserId}", userId);
            throw;
        }
    }

    public async Task UpdateNewsletterPreferenceAsync(string userId, bool subscribe)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            user.NewsletterSubscribed = subscribe;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to update newsletter preference: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update newsletter preference for user {UserId}", userId);
            throw;
        }
    }

    public async Task<List<NewsletterSubscriberDto>> GetNewsletterSubscribersAsync()
    {
        try
        {
            return await _userManager.Users
                .Where(u => u.NewsletterSubscribed)
                .Select(u => new NewsletterSubscriberDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    NewsletterSubscribed = u.NewsletterSubscribed,
                    IsActive = u.IsActive
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get newsletter subscribers");
            throw;
        }
    }
}
