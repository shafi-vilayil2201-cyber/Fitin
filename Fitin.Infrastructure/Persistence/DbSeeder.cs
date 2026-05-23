using Fitin.Domain.Entities;
using Fitin.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Fitin.Domain.Entities.Categories;
using Microsoft.Extensions.Configuration;

namespace Fitin.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(
        AppDbContext context,
        IConfiguration configuration,
        bool isDevelopment)
    {
        var adminEmail = configuration["SeedAdmin:Email"]?.Trim().ToLowerInvariant();
        var adminPassword = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var categories = new List<Category>
        {
            new Category("Running", "https://your-image-url.com/running.jpg"),
        };

        if (!context.Categories.Any()) 
        {
            await context.Categories.AddRangeAsync(categories);
        }

        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == adminEmail);

        if (existingUser == null)
        {
            var admin = new User(
                "System Admin",
                adminEmail,
                BCrypt.Net.BCrypt.HashPassword(adminPassword),
                UserRole.Admin
            );

            await context.Users.AddAsync(admin);
        }
        else
        {
            if (existingUser.Role != UserRole.Admin)
            {
                existingUser.UpdateRole(UserRole.Admin);
            }

            if (!BCrypt.Net.BCrypt.Verify(adminPassword, existingUser.PasswordHash))
            {
                existingUser.SetPasswordHash(BCrypt.Net.BCrypt.HashPassword(adminPassword));
            }
        }

        await context.SaveChangesAsync();
    }
}
