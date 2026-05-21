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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>(entity =>
                {
                    entity.Ignore(e => e.Cover);
                    entity.Ignore(e => e.Thumbnail);
                });

            modelBuilder.Entity<User>()
              .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(u => u.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLikedArticle>(entity =>
            {
                entity.HasKey(ul => new { ul.UserId, ul.ArticleId });

                entity.HasOne(ul => ul.User)
                      .WithMany()
                      .HasForeignKey(ul => ul.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ul => ul.ArticleId);
                entity.Ignore(ul => ul.Article);  
            });
        }
    }
}