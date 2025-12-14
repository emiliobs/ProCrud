using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProCrud.Api.Data;
using ProCrud.Api.Services;
using ProCrud.Shared.DTOs;

namespace ProCrud.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(AppDbContext context, IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<List<ProductDto>> GetAllAsync([FromQuery] string? search) => await service.GetAllAsync(search);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDetailDto>> GetByIdAsync(Guid id)
    {
        var product = await service.GetByIdAsync(id);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(ProductCreateDto productCreateDto)
    {
        var categoryExist = await context.Categories.AnyAsync(c => c.Id == productCreateDto.CategoryId);
        if (!categoryExist)
        {
            return ValidationProblem($"CategoryId {productCreateDto.CategoryId} does not exist.");
        }

        var id = await service.CreateAsync(productCreateDto);

        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, ProductUpdateDto productUpdateDto)
    {
        if (id != productUpdateDto.Id)
        {
            BadRequest("Id in URL does not match Id in Body.");
        }

        var categoryExist = await context.Categories.AnyAsync(c => c.Id == productUpdateDto.CategoryId);
        if (!categoryExist)
        {
            return ValidationProblem($"CategoryId {productUpdateDto.CategoryId} does not exist.");
        }

        var update = await service.UpdateAsync(id, productUpdateDto);

        return Ok(update);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletAsyn(Guid id)
    {
        var deleted = await service.DeleteAsync(id);
        return Ok(deleted);
    }
}