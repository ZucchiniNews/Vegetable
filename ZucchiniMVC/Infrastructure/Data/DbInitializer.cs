using Microsoft.AspNetCore.Identity;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.Data
{
    public class DbInitializer
    {

        public static async Task SeedRoles(RoleManager<Roles> roleManager)
        {
            string[] roles = { "Admin", "Editor", "Writer", "Reader" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new Roles();
                    newRole.Name = role;
                    newRole.NormalizedName = role.ToUpper();
                    await roleManager.CreateAsync(newRole);
                }
            }
        }

        public static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            var defaultUser = new User
            {
                UserName = "ZucchiniNews@gmail.com",
                Email = "ZucchiniNews@gmail.com",
                FirstName = "System",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(defaultUser, "PrettyPenny123(:");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultUser, "Admin");
            }
        }
        public static async Task SeedPlansAsync(ApplicationDbContext context)
        {
            if (!context.SubscriptionPlans.Any())
            {
                var types = new List<SubscriptionPlan>
                {
                    new SubscriptionPlan { Name = "Weekly", Description = "Breaking news access, Weekly recap newsletter", Price = 10, StripePriceId = "price_1TQo13Rz2wduS8uUnwlp5O8A"},
                    new SubscriptionPlan { Name = "Monthly", Description = "Unlimited news access, Daily morning briefings, Ad-free experience", Price = 49, StripePriceId="price_1TQo13Rz2wduS8uUJEcnXnV2"},
                    new SubscriptionPlan { Name = "Yearly", Description = "Save 15% annually, All Monthly benefits, Exclusive in-depth reports", Price = 500, StripePriceId ="price_1TQo13Rz2wduS8uULKNXjMuo"}
                };
                context.SubscriptionPlans.AddRange(types);
                await context.SaveChangesAsync();
            }
        }
    }
}
