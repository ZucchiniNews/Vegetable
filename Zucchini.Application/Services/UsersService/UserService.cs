using Domain.Entities;
using Application.Interfaces;
using Application.Services.Logger;
using Microsoft.Extensions.Logging;

namespace Application.Services.UsersService;

public class UserService : ServiceBase<UserService>, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, ILoggerFactory loggerFactory) 
        : base(loggerFactory)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        try
        {
            return await _userRepository.GetByIdAsync(userId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get user by id {UserId}", userId);
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _userRepository.GetByEmailAsync(email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get user by email {Email}", email);
            throw;
        }
    }

    public async Task<User?> GetUserWithSubscriptionAsync(string userId)
    {
        try
        {
            return await _userRepository.GetWithSubscriptionAsync(userId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get user with subscription {UserId}", userId);
            throw;
        }
    }

    public async Task<User?> GetUserWithRolesAsync(string userId)
    {
        try
        {
            return await _userRepository.GetByIdAsync(userId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get user with roles {UserId}", userId);
            throw;
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            return await _userRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get all users");
            throw;
        }
    }

    public async Task<List<User>> SearchUsersAsync(string searchTerm)
    {
        try
        {
            return await _userRepository.SearchAsync(searchTerm);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to search users with term {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update user {UserId}", user.Id);
            throw;
        }
    }

    public async Task DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            // Soft delete — disable account without removing data
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete user {UserId}", userId);
            throw;
        }
    }

    public async Task AnonymizeUserDataAsync(string userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            user.FirstName = "Deleted";
            user.LastName = "User";
            user.Email = $"deleted_{userId}@anonymized.com";
            user.UserName = $"deleted_{userId}";
            user.PhoneNumber = null;
            user.DateOfBirth = null;
            user.NewsletterSubscribed = false;

            await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to anonymize user {UserId}", userId);
            throw;
        }
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            return await _userRepository.GetRolesAsync(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get roles for user {UserId}", userId);
            throw;
        }
    }

    public async Task AssignRoleAsync(string userId, string roleName)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            if (!await _userRepository.RoleExistsAsync(roleName))
                throw new ArgumentException($"Role {roleName} does not exist.");

            await _userRepository.AddToRoleAsync(user, roleName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to assign role {Role} to user {UserId}", roleName, userId);
            throw;
        }
    }

    public async Task RemoveRoleAsync(string userId, string roleName)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            await _userRepository.RemoveFromRoleAsync(user, roleName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to remove role {Role} from user {UserId}", roleName, userId);
            throw;
        }
    }

    public async Task LockUserAsync(string userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            await _userRepository.LockAsync(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to lock user {UserId}", userId);
            throw;
        }
    }

    public async Task UnlockUserAsync(string userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            await _userRepository.UnlockAsync(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to unlock user {UserId}", userId);
            throw;
        }
    }

    public async Task UpdateNewsletterPreferenceAsync(string userId, bool subscribe)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found.");

            user.NewsletterSubscribed = subscribe;
            await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update newsletter preference for user {UserId}", userId);
            throw;
        }
    }

    public async Task<List<User>> GetNewsletterSubscribersAsync()
    {
        try
        {
            return await _userRepository.GetNewsletterSubscribersAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get newsletter subscribers");
            throw;
        }
    }
}