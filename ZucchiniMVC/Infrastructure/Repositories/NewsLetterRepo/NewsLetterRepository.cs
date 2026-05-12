using Microsoft.EntityFrameworkCore;
using Zucchinimvc.Infrastructure.Data;

namespace Zucchinimvc.Infrastructure.Repositories.NewsLetterRepo
{
    public class NewsLetterRepository : INewsLetterRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsLetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> SwitchNewsLetterSubscriptionAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            user.NewsletterSubscribed = !user.NewsletterSubscribed;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<string>> GetAllUsersEmailsWithActiveNewsLetterAsync()
        {
            return await _context.Users
                .Where(u => u.NewsletterSubscribed)
                .Select(u => u.Email)
                .ToListAsync();
        }
    }
}