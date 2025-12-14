using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProCrud.Shared.DTOs.Entities;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new List<Product>();
}