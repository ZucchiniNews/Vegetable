using Domain.Entities;

namespace Application.Services.UsersService;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(string userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(string userId); // soft delete for GDPR compliance
    Task AnonymizeUserDataAsync(string userId); // for GDPR compliance


    // Role management
    Task<User?> GetUserWithRolesAsync(string userId);
    Task<IList<string>> GetUserRolesAsync(string userId);
    Task AssignRoleAsync(string userId, string roleName);
    Task RemoveRoleAsync(string userId, string roleName);

    // Admin operations
    Task<List<User>> GetAllUsersAsync();
    Task<List<User>> SearchUsersAsync(string searchTerm);
    Task LockUserAsync(string userId);
    Task UnlockUserAsync(string userId);

    Task<User?> GetUserWithSubscriptionAsync(string userId);

    // Newsletter 
    Task UpdateNewsletterPreferenceAsync(string userId, bool subscribe);
    Task<List<User>> GetNewsletterSubscribersAsync();
}