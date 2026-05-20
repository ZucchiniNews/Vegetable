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
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserLikedArticle> UserLikedArticles { get; set; }
        public DbSet<BillingAccount> BillingAccounts { get; set; }
        public DbSet<CurrencyHistoryEntity> CurrencyHistory { get; set; }

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

            modelBuilder.Entity<User>()
              .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<UserLikedArticle>()
                .HasOne(u => u.User)
                .WithMany(u => u.LikedArticles)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(u => u.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}