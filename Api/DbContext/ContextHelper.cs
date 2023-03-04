using Api.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.DbContext;

public static class ContextHelper
{
    public static async Task Seeding(DocumentFlowDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        if (!roleManager.Roles.Where(p=>p.NormalizedName.Equals("Admin")).Any())
        {
            var adminRole = new IdentityRole<int>
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };
            await roleManager.CreateAsync(adminRole);
        }
        if (!roleManager.Roles.Where(p=>p.NormalizedName.Equals("User")).Any())
        {
            var userRole = new IdentityRole<int>
            {
                Name = "User",
                NormalizedName = "USER"
            };
            await roleManager.CreateAsync(userRole);
        }
        if (!userManager.Users.Where(p=>p.UserName.Equals("Admin")).Any())
        {
            var adminUser = new User
            {
                UserName = "Admin",
                Email = "Admin@mail.ru"
            };
            var result = await userManager.CreateAsync(adminUser, "password");

            if (result.Succeeded)
            {
                var role = await roleManager.FindByNameAsync("Admin");

                await userManager.AddToRoleAsync(await userManager.FindByNameAsync("Admin"), role.Name);
            }
        }
    }
}