using Masaafa.Application.Common.Identity;
using Masaafa.Application.Common.Notifications;
using Masaafa.Application.Services;
using Masaafa.Infrastructure.Common.Identity;
using Masaafa.Infrastructure.Common.Notifications;
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
        // Services
        services
            .AddScoped<IClientService, ClientService>()
            .AddScoped<IEmployeeService, EmployeeService>()
            .AddScoped<IItemService, ItemService>()
            .AddScoped<IItemGroupService, ItemGroupService>()
            .AddScoped<IWarehouseItemService, WarehouseItemService>()
            .AddScoped<IWarehouseService, WarehouseService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<ITokenGeneratorService, JwtTokenGeneratorService>()
            .AddScoped<IInventoryService, InventoryService>()
            .AddScoped<IInventoryItemService, InventoryItemService>()
            .AddScoped<ITransferRequestItemService, TransferRequestItemService>()
            .AddScoped<ITransferRequestService, TransferRequestService>()
            .AddScoped<ISalesOrderItemService, SalesOrderItemService>()
            .AddScoped<ISalesOrderService, SalesOrderService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IBalanceService, BalanceService>()
            .AddSingleton<ICommandDispatcherService, CommandDispatcherService>()
            .AddScoped<IMessageSenderService, MessageSenderService>()
            .AddScoped<ISalesOrderMessageRenderingService, SalesOrderMessageRenderingService>()
            .AddScoped<IPaymentMessageRenderingService, PaymentMessageRenderingService>();
    }
}