using ProCrud.Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ProCrud.Client.Services
{
    public class ProductsApi(HttpClient httpClient)
    {
        public async Task<List<ProductDto>> GetAllAsync(string? search)
        {
            var url = string.IsNullOrWhiteSpace(search) ? "api/products" : $"{Uri.EscapeDataString(search)}";

            return await httpClient.GetFromJsonAsync<List<ProductDto>>(url);
        }

        public async Task<ProductDetailDto?> GetByIdAsync(Guid id) => await
            httpClient.GetFromJsonAsync<ProductDetailDto>($"api/products/{id}");

        public async Task<Guid> CreateAsync(ProductCreateDto productCreateDto)
        {
            var response = await httpClient.PostAsJsonAsync("api/products", productCreateDto);

            response.EnsureSuccessStatusCode();

            var id = await response.Content.ReadFromJsonAsync<Guid>();

            if (id == Guid.Empty)
            {
                throw new InvalidOperationException("The API returnet an empty Guid.");
            }

            return id;
        }

        public async Task UpdateAsync(Guid id, ProductUpdateDto productUpdateDto)
        {
            var response = await httpClient.PutAsJsonAsync($"api/products/{id}", productUpdateDto);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await httpClient.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}