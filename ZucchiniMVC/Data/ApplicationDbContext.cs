using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zucchinimvc.Models; // adjust if your models are in another namespace

namespace Zucchinimvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    //    public DbSet<Article> Articles { get; set; }
    //    public DbSet<Category> Categories { get; set; }
    //    public DbSet<Subscription> Subscriptions { get; set; }
    //
    }
}