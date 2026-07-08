using Fitin.Domain.Entities;
using Fitin.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Fitin.Domain.Entities.Categories;
using Fitin.Domain.Entities.Products;
using Fitin.Domain.Entities.Supplements;
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

        // 1. Seed Categories
        var categories = new List<Category>
        {
            new Category("Running", "https://images.unsplash.com/photo-147649763962-0c623066013b?auto=format&fit=crop&w=1200&q=80"),
            new Category("Training", "https://images.unsplash.com/photo-1517838277536-f5f99be501cd?auto=format&fit=crop&w=1200&q=80"),
            new Category("Protein", "https://images.unsplash.com/photo-1607619056574-7b8d3ee536b2?auto=format&fit=crop&w=900&q=80"),
            new Category("Energy", "https://images.unsplash.com/photo-1593095948071-474c5cc2989d?auto=format&fit=crop&w=900&q=80"),
            new Category("Wellness", "https://images.unsplash.com/photo-1579722821273-0f6c4d44362f?auto=format&fit=crop&w=900&q=80"),
            new Category("Hydration", "https://images.unsplash.com/photo-1622484212850-eb596d769edc?auto=format&fit=crop&w=900&q=80"),
            new Category("Recovery", "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?auto=format&fit=crop&w=900&q=80")
        };

        foreach (var cat in categories)
        {
            if (!await context.Categories.AnyAsync(c => c.Name == cat.Name))
            {
                await context.Categories.AddAsync(cat);
            }
        }
        await context.SaveChangesAsync();

        var dbCategories = await context.Categories.ToListAsync();
        var categoryMap = dbCategories.ToDictionary(c => c.Name.ToLowerInvariant(), c => c.Id);

        // 2. Seed Products (Sports Gear)
        var products = new List<Product>
        {
            new Product(
                "Ultralight Carbon Running Shoes",
                7999m,
                categoryMap["running"],
                25,
                "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=900&q=80",
                "SpeedForm",
                "Running",
                "Advanced running shoes with a responsive carbon fiber plate and breathable mesh.",
                "Responsive carbon plate shoes.",
                "Engineered with a full-length carbon fiber plate and high-rebound cushioning to deliver maximum propulsion and energy return for marathon or track runs.",
                4.9m,
                0m
            ),
            new Product(
                "All-Weather Training Backpack",
                3499m,
                categoryMap["training"],
                30,
                "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?auto=format&fit=crop&w=900&q=80",
                "FitN Gear",
                "Training",
                "Durable water-resistant training bag with dedicated shoe and laptop compartments.",
                "Water-resistant utility backpack.",
                "Built with heavy-duty ballistic nylon and waterproof zippers. Includes an isolated bottom shoe pocket, wet/dry laundry separator, and padded sleeve.",
                4.7m,
                10m
            ),
            new Product(
                "Adjustable Smart Dumbbell Set",
                12999m,
                categoryMap["training"],
                15,
                "https://images.unsplash.com/photo-1638536532686-d610adfc8e5c?auto=format&fit=crop&w=900&q=80",
                "IronForge",
                "Training",
                "Space-saving adjustable dumbbells ranging from 2kg to 24kg per hand.",
                "Compact adjustable smart dumbbells.",
                "Replace 15 separate dumbbell pairs with a single dial-select system. Heavy-duty textured steel handle provides secure non-slip grip.",
                4.8m,
                15m
            ),
            new Product(
                "Pro Grip Training Gloves",
                1199m,
                categoryMap["training"],
                50,
                "https://images.unsplash.com/photo-1517838277536-f5f99be501cd?auto=format&fit=crop&w=900&q=80",
                "GripMaster",
                "Training",
                "Padded training gloves with integrated wrist support wraps.",
                "Comfort-padded lifting gloves.",
                "Premium leather palms with gel padding to prevent calluses and blisters, paired with elastic wrist wraps for stabilizing heavy lifts.",
                4.5m,
                5m
            )
        };

        foreach (var prod in products)
        {
            if (!await context.Products.AnyAsync(p => p.Name == prod.Name))
            {
                await context.Products.AddAsync(prod);
            }
        }
        await context.SaveChangesAsync();

        // 3. Seed Supplements (Health Supplements)
        var supplements = new List<Supplement>
        {
            new Supplement(
                "Whey Protein Isolate",
                2499m,
                categoryMap["protein"],
                50,
                "https://images.unsplash.com/photo-1607619056574-7b8d3ee536b2?auto=format&fit=crop&w=900&q=80",
                "FitN Labs",
                "Elite clean filtered whey protein isolate for rapid recovery and muscle growth.",
                "Elite ultra-filtered recovery blend.",
                "Premium ultra-filtered whey isolate featuring 25g of protein, 0g sugar, and essential BCAAs per serving. Designed for immediate absorption post-workout.",
                4.8m,
                10m
            ),
            new Supplement(
                "Pre-Workout Focus",
                1899m,
                categoryMap["energy"],
                40,
                "https://images.unsplash.com/photo-1593095948071-474c5cc2989d?auto=format&fit=crop&w=900&q=80",
                "Core Fuel",
                "High-octane pre-workout focus blend to enhance energy and endurance.",
                "Explosive citrus charge pre-workout.",
                "Formulated with premium L-Citrulline, Beta-Alanine, and clean natural caffeine to maximize blood flow, focus, and athletic stamina without the crash.",
                4.7m,
                5m
            ),
            new Supplement(
                "Daily Multivitamin",
                1299m,
                categoryMap["wellness"],
                60,
                "https://images.unsplash.com/photo-1579722821273-0f6c4d44362f?auto=format&fit=crop&w=900&q=80",
                "WellForm",
                "Complete daily vitamin and mineral support for active athletes.",
                "Daily comprehensive micronutrient support.",
                "Includes 24 essential vitamins and minerals tailored to support metabolic performance, joint health, immune function, and cellular energy.",
                4.9m,
                0m
            ),
            new Supplement(
                "Hydration Electro Mix",
                999m,
                categoryMap["hydration"],
                85,
                "https://images.unsplash.com/photo-1622484212850-eb596d769edc?auto=format&fit=crop&w=900&q=80",
                "Pulse Hydrate",
                "Rapid electrolyte replenishment drink mix for peak endurance.",
                "Endurance-ready rapid hydration.",
                "Scientifically balanced ratio of sodium, potassium, and magnesium to prevent cramping and maintain fluid balance during intense training sessions.",
                4.6m,
                15m
            ),
            new Supplement(
                "Plant Protein Blend",
                2199m,
                categoryMap["protein"],
                35,
                "https://images.unsplash.com/photo-1610725664285-7c57e6eeac3f?auto=format&fit=crop&w=900&q=80",
                "Nature Lift",
                "All-natural vegan plant protein blend with smooth vanilla flavor.",
                "Organic pea & brown rice protein.",
                "Organic plant-based protein featuring pea, hemp, and chia seeds. Provides a complete amino acid profile, high digestibility, and delicious natural vanilla flavor.",
                4.5m,
                0m
            ),
            new Supplement(
                "Omega Recovery Caps",
                1499m,
                categoryMap["recovery"],
                45,
                "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?auto=format&fit=crop&w=900&q=80",
                "MoveWell",
                "High-potency omega-3 recovery caps for joint comfort and cardiovascular support.",
                "Triple-strength EPA/DHA recovery caps.",
                "Provides 1200mg of active EPA and DHA per serving. Reduces exercise-induced joint inflammation, supports muscle protein synthesis, and boosts cognitive focus.",
                4.8m,
                12m
            )
        };

        foreach (var supp in supplements)
        {
            if (!await context.Supplements.AnyAsync(s => s.Name == supp.Name))
            {
                await context.Supplements.AddAsync(supp);
            }
        }

        // 3. Seed Admin User
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
