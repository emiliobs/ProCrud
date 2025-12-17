using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProCrud.Client.Services;
using ProCrud.Shared.DTOs;
using ProCrud.Shared.Enums;

namespace ProCrud.Client.Pages.Products;

public partial class ProductDialog(ProductsApi productsApi)
{
    [CascadingParameter] private IMudDialogInstance Dialog { get; set; } = default!;

    [Parameter] public string Title { get; set; } = "Producto";

    // Inicializamos con lista vacía para evitar crash
    [Parameter] public List<CategoryDto> Categories { get; set; } = [];

    [Parameter] public ProductDIalogMode Mode { get; set; }
    [Parameter] public Guid ProductId { get; set; }
    [Parameter] public ProductDetailDto? Model { get; set; }

    private ProductDetailDto _editModel = new();

    // Usamos OnInitialized síncrono para asignar los datos
    protected override void OnInitialized()
    {
        if (Mode == ProductDIalogMode.Edit && Model is not null)
        {
            _editModel = new ProductDetailDto
            {
                Id = Model.Id,
                Name = Model.Name,
                Description = Model.Description,
                Price = Model.Price,
                IsActive = Model.IsActive,
                CategoryId = Model.CategoryId
            };
        }
        else
        {
            // Modo Crear: Valores por defecto
            _editModel = new ProductDetailDto
            {
                IsActive = true,
                // Selecciona la primera categoría por defecto si existe
                CategoryId = Categories.FirstOrDefault()?.Id ?? Guid.Empty
            };
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            if (Mode == ProductDIalogMode.Create)
            {
                var dto = new ProductCreateDto
                {
                    Name = _editModel.Name,
                    Description = _editModel.Description,
                    Price = _editModel.Price,
                    IsActive = _editModel.IsActive,
                    CategoryId = _editModel.CategoryId
                };
                await productsApi.CreateAsync(dto);
            }
            else
            {
                var dto = new ProductUpdateDto
                {
                    Name = _editModel.Name,
                    Description = _editModel.Description,
                    Price = _editModel.Price,
                    IsActive = _editModel.IsActive,
                    CategoryId = _editModel.CategoryId
                };
                await productsApi.UpdateAsync(ProductId, dto);
            }

            Dialog.Close(DialogResult.Ok(true));
        }
        catch (Exception)
        {
            // Aquí podrías mostrar un error si falla la API
        }
    }

    private void CancelAsync() => Dialog.Cancel();
}