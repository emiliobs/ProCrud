using MudBlazor;
using ProCrud.Shared.DTOs;

namespace ProCrud.Client.Pages.Categories;

public partial class CategoryPage
{
    private List<CategoryDto> _all = [];
    private List<CategoryDto> _filtered = [];
    private bool _isLoading = true;
    private string? _search;

    protected override async Task OnInitializedAsync()
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        try
        {
            _isLoading = true;
            _all = (await CategoriesApi.GetAllAsync()).ToList();
            await ApplyFilterAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load categories: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private Task ApplyFilterAsync()
    {
        if (string.IsNullOrWhiteSpace(_search))
        {
            _filtered = _all
                .OrderBy(c => c.Name)
                .ToList();

            return Task.CompletedTask;
        }

        var term = _search.Trim();
        _filtered = _all
            .Where(c => c.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.Name)
            .ToList();

        return Task.CompletedTask;
    }
}