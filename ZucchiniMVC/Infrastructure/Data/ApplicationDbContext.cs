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

        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<SubscriptionPlan> Plans { get; set; }
        public DbSet<UserLikedArticle> UserLikedArticles { get; set; }
        public DbSet<BillingAccount> BillingAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>(entity =>
                {
                    entity.Ignore(e => e.Cover);
                    entity.Ignore(e => e.Thumbnail);
                });

            modelBuilder.Entity<UserLikedArticle>()
              .HasKey(ul => new { ul.ArticleId, ul.UserId });

        }
    }
}