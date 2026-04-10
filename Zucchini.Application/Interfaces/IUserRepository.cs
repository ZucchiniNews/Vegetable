using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string userId);
    Task<User?> GetByEmailAsync(string email);
    Task UpdateAsync(User user);
    Task<IList<string>> GetRolesAsync(User user);
    Task AddToRoleAsync(User user, string roleName);
    Task RemoveFromRoleAsync(User user, string roleName);
    Task<List<User>> GetAllAsync();
    Task<List<User>> SearchAsync(string searchTerm);
    Task LockAsync(User user);
    Task UnlockAsync(User user);
    Task<User?> GetWithSubscriptionAsync(string userId);
    Task<List<User>> GetNewsletterSubscribersAsync();
}
