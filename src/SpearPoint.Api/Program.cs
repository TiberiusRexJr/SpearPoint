using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpearPoint.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
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

// If/when you add auth later
// app.UseAuthentication();
// app.UseAuthorization();
//AddInfrastructure(Configuration)
app.MapControllers();
app.MapHealthChecks("/healthz");
app.Run();
