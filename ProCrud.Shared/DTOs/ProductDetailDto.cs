using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProCrud.Shared.DTOs;

public class ProductDetailDto
{
    public Guid Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(600)]
    public string? Description { get; set; }

    [Range(0, 1_000_000)]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public Guid CategoryId { get; set; }
}