using Microsoft.EntityFrameworkCore;
using ProCrud.Api.Data;
using ProCrud.Shared.DTOs;
using ProCrud.Shared.DTOs.Entities;

namespace ProCrud.Api.Services;

public class ProductService(AppDbContext context) : IProductService
{
    private readonly AppDbContext _context = context;

    public async Task<List<ProductDto>> GetAllAsync(string? search)
    {
        var query = _context.Products.AsNoTracking().Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var getSearch = search.Trim();
            query = query.Where(p => p.Name.Contains(getSearch));
        }

        return await query.OrderBy(p => p.Name).Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            CategoryName = p.Category != null ? p.Category.Name : string.Empty,
            Price = p.Price,
            IsActive = p.IsActive
        }).ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Products.AsNoTracking().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

        if (entity is null)
        {
            return null;
        }

        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            CategoryName = entity.Category != null ? entity.Category.Name : string.Empty,
            Price = entity.Price,
            IsActive = entity.IsActive,
        };
    }

    public async Task<Guid> CreateAsync(ProductCreateDto productCreateDto)
    {
        var entity = new Product
        {
            Name = productCreateDto.Name,
            CategoryId = productCreateDto.CategoryId,
            Price = productCreateDto.Price,
            IsActive = productCreateDto.IsActive,
            Description = productCreateDto.Description?.Trim(),
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, ProductUpdateDto productUpdateDto)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        entity.Name = productUpdateDto.Name;
        entity.Description = productUpdateDto.Description?.Trim();
        entity.Price = productUpdateDto.Price;
        entity.IsActive = productUpdateDto.IsActive;
        entity.CategoryId = productUpdateDto.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}