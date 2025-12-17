using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ProCrud.Client;
using ProCrud.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? throw new InvalidOperationException("API base URL is not configured.");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

var apiURL = "https://localhost:7241/";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiURL)
});

builder.Services.AddMudServices();

builder.Services.AddScoped<CategoriesApi>();
builder.Services.AddScoped<ProductsApi>(); // <--- UNCOMMENT THIS LINE

await builder.Build().RunAsync();