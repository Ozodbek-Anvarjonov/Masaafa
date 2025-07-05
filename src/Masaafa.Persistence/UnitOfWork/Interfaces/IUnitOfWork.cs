using Masaafa.Persistence.Repositories.Interfaces;

namespace Masaafa.Persistence.UnitOfWork.Interfaces;

public interface IUnitOfWork : ITransactionManager, IDisposable, IAsyncDisposable
{
    // First-tier
    IClientRepository Clients { get; }
    IEmployeeRepository Employees { get; }

    IInventoryRepository Inventories { get; }
    IInventoryItemRepository InventoryItems { get; }

    // Second-tier
    IItemRepository Items { get; }
    IItemGroupRepository ItemGroups { get; }

    IWarehouseRepository Warehouses { get; }
    IWarehouseItemRepository WarehouseItems { get; }

    ITransferRequestRepository TransferRequests { get; }
    ITransferRequestItemRepository TransferRequestItems { get; }

    ISalesOrderRepository SalesOrders { get; }
    ISalesOrderItemRepository SalesOrderItems { get; }

    IPaymentRepository Payments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}