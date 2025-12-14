using Microsoft.EntityFrameworkCore;
using ProCrud.Shared.DTOs.Entities;

namespace ProCrud.Api.Data;

public class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider service)
    {
        using var scope = service.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        if (await db.Products.AnyAsync())
        {
            return;
        }

        List<Category> categories = new()
        {
            new Category { Name = "Hardware" },
            new Category { Name = "Software" },
            new Category { Name = "Services" }
        };

        db.Categories.AddRange(categories);
        await db.SaveChangesAsync();

        var hardware = categories.Single(c => c.Name == "Hardware").Id;
        var software = categories.Single(c => c.Name == "Software").Id;
        var services = categories.Single(c => c.Name == "Services").Id;

        var products = new List<Product>
        {
             // Hardware (5)
            new() { Name = "Mechanical Keyboard", Description = "87-key, hot-swappable switches.", Price = 129.99m, IsActive = true, CategoryId = hardware },
            new() { Name = "Ergonomic Mouse", Description = "Vertical mouse for wrist comfort.", Price = 59.50m, IsActive = true, CategoryId = hardware },
            new() { Name = "USB-C Dock", Description = "Multiport dock with HDMI and Ethernet.", Price = 89.00m, IsActive = true, CategoryId = hardware },
            new() { Name = "27\" Monitor", Description = "QHD IPS monitor.", Price = 229.00m, IsActive = true, CategoryId = hardware },
            new() { Name = "Noise-cancelling Headset", Description = "Office headset with ENC mic.", Price = 149.00m, IsActive = true, CategoryId = hardware },

            // Software (5)
            new() { Name = "Time Tracker Pro", Description = "Team time tracking and reporting.", Price = 12.00m, IsActive = true, CategoryId = software },
            new() { Name = "Invoice Manager", Description = "Invoicing with templates and reminders.", Price = 15.00m, IsActive = true, CategoryId = software },
            new() { Name = "Password Vault", Description = "Secure password management.", Price = 8.99m, IsActive = true, CategoryId = software },
            new() { Name = "Note Taking Suite", Description = "Markdown notes with sync.", Price = 6.50m, IsActive = true, CategoryId = software },
            new() { Name = "CRM Starter", Description = "Lightweight CRM for small teams.", Price = 19.00m, IsActive = false, CategoryId = software },

            // Services (5)
            new() { Name = "Onboarding Session", Description = "90-minute onboarding call.", Price = 99.00m, IsActive = true, CategoryId = services },
            new() { Name = "Priority Support", Description = "Email + chat, 4h response time.", Price = 49.00m, IsActive = true, CategoryId = services },
            new() { Name = "Data Migration", Description = "Import legacy data to new system.", Price = 399.00m, IsActive = true, CategoryId = services },
            new() { Name = "Custom Integration", Description = "API integration service.", Price = 799.00m, IsActive = true, CategoryId = services },
            new() { Name = "Security Review", Description = "Light security posture review.", Price = 299.00m, IsActive = false, CategoryId = services },
        };

        db.Products.AddRange(products);
        await db.SaveChangesAsync();
    }
}