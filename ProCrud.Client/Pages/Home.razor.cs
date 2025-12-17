using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProCrud.Client.Pages.Products; // Asegúrate de que este namespace coincida con donde pongas ProductDialog
using ProCrud.Client.Services;
using ProCrud.Shared.DTOs;
using ProCrud.Shared.Enums;

namespace ProCrud.Client.Pages;

public partial class Home(CategoriesApi _categoriesApi, ProductsApi _productsApi)
{
    // Inicializamos listas vacías para evitar nulls
    private List<ProductDto> _items = [];

    private List<CategoryDto> _categories = [];

    private string? _search;
    private bool _loading;

    // Inyectamos los servicios de MudBlazor aquí (NO en el archivo .razor)
    [Inject] private IDialogService DialogService { get; set; } = default!;

    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        try
        {
            // Cargamos categorías primero
            var cats = await _categoriesApi.GetAllAsync();
            if (cats is not null) _categories = cats;

            await LoadAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error al iniciar: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnSearchChanged()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _loading = true;
        try
        {
            // Protección contra nulos
            var result = await _productsApi.GetAllAsync(_search);
            _items = result ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error cargando productos: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    private async Task CreateAsync()
    {
        var parameters = new DialogParameters<ProductDialog>
        {
            { x => x.Title, "Crear Producto" },
            { x => x.Categories, _categories }, // Pasamos las categorías cargadas
            { x => x.Mode, ProductDIalogMode.Create }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ProductDialog>("", parameters, options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            Snackbar.Add("Producto creado correctamente.", Severity.Success);
            await LoadAsync();
        }
    }

    private async Task EditAsync(Guid id)
    {
        var detail = await _productsApi.GetByIdAsync(id);
        if (detail is null)
        {
            Snackbar.Add("No se encontró el producto.", Severity.Error);
            return;
        }

        var parameters = new DialogParameters<ProductDialog>
        {
            { x => x.Title, "Editar Producto" },
            { x => x.Categories, _categories },
            { x => x.Mode, ProductDIalogMode.Edit},
            { x => x.ProductId, id },
            { x => x.Model, detail}
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ProductDialog>("", parameters, options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            Snackbar.Add("Producto actualizado correctamente.", Severity.Success);
            await LoadAsync();
        }
    }

    private async Task DeleteAsync(Guid id)
    {
        var confirm = await DialogService.ShowMessageBox(
             "Eliminar producto",
             "¿Estás seguro de eliminar este producto? No se puede deshacer.",
             yesText: "Eliminar", cancelText: "Cancelar");

        if (confirm == true)
        {
            try
            {
                await _productsApi.DeleteAsync(id);
                Snackbar.Add("Producto eliminado.", Severity.Success);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al eliminar: {ex.Message}", Severity.Error);
            }
        }
    }
}