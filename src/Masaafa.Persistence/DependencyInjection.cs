using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Interceptors;
using Masaafa.Persistence.Repositories;
using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Masaafa.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddServices();

        return services;
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableInterceptor>();
        services.AddScoped<SoftDeletedInterceptor>();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var auditableInterceptor = provider.GetRequiredService<AuditableInterceptor>();
            var softDeletedInterceptor = provider.GetRequiredService<SoftDeletedInterceptor>();

            options
                .UseNpgsql(configuration.GetConnectionString("DefaultDbConnection"))
                .AddInterceptors(auditableInterceptor)
                .AddInterceptors(softDeletedInterceptor);
        });
    }

    private static void AddServices(this IServiceCollection services)
    {
        // Unit Of Work
        services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
    }
}