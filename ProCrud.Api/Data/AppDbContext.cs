using Microsoft.EntityFrameworkCore;
using ProCrud.Shared.DTOs.Entities;

namespace ProCrud.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(p =>
        {
            p.Property(x => x.Price).HasPrecision(18, 2);
            p.HasIndex(x => x.Name);
        });

        modelBuilder.Entity<Category>(c =>
        {
            c.HasIndex(x => x.Name).IsUnique();
        });

        //modelBuilder.Entity<Product>()
        //    .HasOne(p => p.Category)
        //    .WithMany(c => c.Products)
        //    .HasForeignKey(p => p.CategoryId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<Product>().HasData(
        //     new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop.", Price = 999.99m, CategoryId = 1 },
        //     new Product { Id = 2, Name = "Smartphone", Description = "A latest model smartphone.", Price = 699.99m, CategoryId = 1 },
        //     new Product { Id = 3, Name = "Novel", Description = "A bestselling novel.", Price = 19.99m, CategoryId = 2 }
        // );

        //modelBuilder.Entity<Category>().HasData(
        //    new Category { Id = 1, Name = "Electronics" },
        //    new Category { Id = 2, Name = "Books" },
        //    new Category { Id = 3, Name = "Clothing" }
        //);
    }
}