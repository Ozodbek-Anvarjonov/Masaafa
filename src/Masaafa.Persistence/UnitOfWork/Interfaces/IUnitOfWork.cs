using Masaafa.Persistence.Repositories.Interfaces;

namespace Masaafa.Persistence.UnitOfWork.Interfaces;

public interface IUnitOfWork : ITransactionManager, IDisposable, IAsyncDisposable
{
    // First-tier
    IClientRepository Clients { get; }
    IEmployeeRepository Employees { get; }

    // Second-tier
    IItemRepository Items { get; }
    IItemGroupRepository ItemGroups { get; }

    IWarehouseRepository Warehouses { get; }
    IWarehouseItemRepository WarehouseItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}