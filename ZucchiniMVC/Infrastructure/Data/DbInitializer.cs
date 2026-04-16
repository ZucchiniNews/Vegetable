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
                    await roleManager.CreateAsync(new Roles {Name = role});
                }
            }
        }

        public static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            var defaultUser = new User
            {
                UserName = "Zucchini_Admin",
                Email = "ZucchiniNews@gmail.com",
                FirstName = "System",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "PrettyPenny;)");  // password ;)
                await userManager.AddToRoleAsync(defaultUser, "Admin");
            }
        }
    }
}
