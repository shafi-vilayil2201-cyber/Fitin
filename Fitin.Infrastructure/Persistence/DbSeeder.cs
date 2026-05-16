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
        if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            return;

        var adminEmail = configuration["SeedAdmin:Email"];
        var adminPassword = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword);

        var admin = new User(
            "System Admin",
            adminEmail,
            passwordHash,
            UserRole.Admin
        );
        var categories = new List<Category>
        {
            new Category("Running", "https://your-image-url.com/running.jpg"),
        };

        if (!context.Categories.Any()) 
        {
            await context.Categories.AddRangeAsync(categories);
        }

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
    }
}
