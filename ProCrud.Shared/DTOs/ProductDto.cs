using System;
using System.Collections.Generic;
using System.Text;

namespace ProCrud.Shared.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}