using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Roles, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<UserLikedArticle> UserLikedArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}