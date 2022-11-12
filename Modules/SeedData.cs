using meta_menu_be.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace meta_menu_be.Modules
{
    public static class SeedData
    {
        public static async void Initialize(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = new string[] { "Admin", "user"};

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role) { NormalizedName = role.ToUpper()});
                }
            }


            var user = new ApplicationUser
            {
                UserName = "Admin",
                Email = "nikolas9924@gmail.com",
                NormalizedEmail = "NIKOLAS9924@GMAIL.COM",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "P@ssw0rd");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(user);

            }

            await AssignRoles(app, user.Email);

            await context.SaveChangesAsync();
        }

        private static async Task<IdentityResult> AssignRoles(WebApplication app, string email)
        {
            using var scope = app.Services.CreateScope();
            UserManager<ApplicationUser> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRoleAsync(user, "Admin");

            return result;
        }
    }
}
