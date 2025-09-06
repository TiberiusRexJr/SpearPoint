using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpearPoint.Application.Abstractions;
using SpearPoint.Infrastructure.Persistence;
using SpearPoint.Infrastructure.Repositories;

namespace SpearPoint.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(cfg.GetConnectionString("Default")));


        services.AddScoped<IQuestionRepository, QuestionRepository>();
        return services;
    }
}
