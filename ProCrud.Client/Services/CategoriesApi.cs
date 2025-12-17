using ProCrud.Shared.DTOs;
using System.Net.Http.Json;

namespace ProCrud.Client.Services;

public class CategoriesApi(HttpClient httpClient)
{
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");
    }
}