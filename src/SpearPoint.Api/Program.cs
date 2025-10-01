using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpearPoint.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
// CORS: allow localhost:4200 (dev) and your SWA domain (prod)
var allowedOrigins = new[] {
    "http://localhost:4200",                 // dev
    "https://<your-swa-domain>.azurestaticapps.net" // prod SWA
};

// Add services
builder.Services.AddCors(o => o.AddPolicy("Spa",
    p => p.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
// Optional: health checks (nice for CI/CD probes)
builder.Services.AddHealthChecks();

var app = builder.Build();

// Dev Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Spa");
// If/when you add auth later
// app.UseAuthentication();
// app.UseAuthorization();
//AddInfrastructure(Configuration)
app.MapControllers();
app.MapHealthChecks("/healthz");
app.Run();
