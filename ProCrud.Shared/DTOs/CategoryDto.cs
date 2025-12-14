using System;
using System.Collections.Generic;
using System.Text;

namespace ProCrud.Shared.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}