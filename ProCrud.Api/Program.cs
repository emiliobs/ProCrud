using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProCrud.Api.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

//OpernAPI support
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sql => sql.EnableRetryOnFailure());
});

builder.Services.AddCors(Options =>
{
    Options.AddDefaultPolicy(p => p.AllowAnyMethod()
                                   .AllowAnyHeader()
                                   .AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors();

// Initialize the database
await DbInitializer.InitializeAsync(app.Services);

app.Run();