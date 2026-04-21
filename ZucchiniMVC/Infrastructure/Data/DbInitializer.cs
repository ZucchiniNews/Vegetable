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

            var result = await userManager.CreateAsync(defaultUser, "Zucchi26?");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultUser, "Admin");
            }
        }
    }
}
