using Masaafa.Application.Common.Identity;
using Masaafa.Application.Services;
using Masaafa.Infrastructure.Common.Identity;
using Masaafa.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Masaafa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);

        return services;
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Identity services
        //services.AddScoped<IUserContext, UserContext>();


        // Services
        services
            .AddScoped<IClientService, ClientService>()
            .AddScoped<IEmployeeService, EmployeeService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<ITokenGeneratorService, JwtTokenGeneratorService>();
    }
}