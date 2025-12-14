using ProCrud.Shared.DTOs;

namespace ProCrud.Api.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync(string? search);

    Task<ProductDto?> GetByIdAsync(Guid id);

    Task<Guid> CreateAsync(ProductCreateDto productCreateDto);

    Task<bool> UpdateAsync(Guid id, ProductUpdateDto productUpdateDto);

    Task<bool> DeleteAsync(Guid id);
}