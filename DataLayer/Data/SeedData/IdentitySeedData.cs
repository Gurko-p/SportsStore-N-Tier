using DataLayer.Data.Contexts;
using DataLayer.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Data.SeedData;

public static class IdentitySeedData
{
    public static async Task EnsurePopulatedAsync(IApplicationBuilder app, IConfiguration configuration)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<IdentityApplicationDbContext>();

        await context.Database.MigrateAsync();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        string adminEmail = configuration["Credentials:Users:admin:email"]!;
        string adminPassword = configuration["Credentials:Users:admin:password"]!;

        var defaultRoles = configuration["Credentials:Roles"]!.Split(";");

        foreach (var role in defaultRoles)
        {
            // Создание ролей
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Создание пользователя admin
        if (!userManager.Users.Any(u => u.UserName == adminEmail))
        {
            var adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
            await userManager.CreateAsync(adminUser, adminPassword);
            await userManager.AddToRoleAsync(adminUser, "admin");
        }
    }
}