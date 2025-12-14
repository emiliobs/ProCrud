using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProCrud.Shared.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(600)]
    public string? Description { get; set; }

    [Required, Range(0, 1_000_000)]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    [Display(Name = "Category")]
    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }
}