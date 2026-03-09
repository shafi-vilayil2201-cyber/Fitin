using Fitin.Domain.Entities;
using Fitin.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fitin.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            return;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");

        var admin = new User(
            "System Admin",
            "admin@fitin.com",
            passwordHash,
            UserRole.Admin
        );

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
    }
}