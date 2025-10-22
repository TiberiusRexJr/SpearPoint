using Microsoft.EntityFrameworkCore;
using SpearPoint.Application.Abstractions;
using SpearPoint.Infrastructure.Extensions;
using SpearPoint.Infrastructure.Persistence;
using SpearPoint.Infrastructure.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers & JSON (enums as strings; camelCase is default)
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Swagger & API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Choose provider from config
var provider = builder.Configuration["Database:Provider"] ?? "Sqlite";

// EF Core – SQLite (file-based by default; tests can override)
builder.Services.AddDbContext<SpearPointDbContext>(opt =>
{
    if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        // needs Microsoft.EntityFrameworkCore.SqlServer
        var cs = builder.Configuration.GetConnectionString("SqlServer");
        opt.UseSqlServer(cs);
    }
    else
    {
        // default to SQLite (needs Microsoft.EntityFrameworkCore.Sqlite)
        var cs = builder.Configuration.GetConnectionString("Sqlite") ?? "Data Source=spearpoint.db";
        opt.UseSqlite(cs);
    }
});

// DI – use fake generator by default; swap via config for prod/real
builder.Services.AddScoped<IQuestionGenerator, FakeQuestionGenerator>();

// CORS – allow localhost (dev) and SWA domain (prod)
var allowedOrigins = new[]
{
    "http://localhost:4200",                         // dev
    "https://<your-swa-domain>.azurestaticapps.net" // prod SWA
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("Spa", policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod());
});

// Infrastructure & health checks
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();

// Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Spa");

app.MapControllers();

// Health endpoints
app.MapHealthChecks("/healthz");
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();

// Required for WebApplicationFactory<T> in tests
public partial class Program { }
