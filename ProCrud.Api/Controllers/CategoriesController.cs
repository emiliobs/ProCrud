using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProCrud.Api.Data;
using ProCrud.Shared.DTOs;

namespace ProCrud.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CategoryDto>> GetAll()
    {
        var categories = await context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return Ok(categories);
    }
}