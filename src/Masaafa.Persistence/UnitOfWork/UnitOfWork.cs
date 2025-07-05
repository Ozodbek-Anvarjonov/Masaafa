using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Masaafa.Persistence.UnitOfWork;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;
    private IDbContextTransaction? _currentTransaction;
    private bool _disposed;

    public bool HasActiveTransaction => _currentTransaction != null;

    public UnitOfWork(
        TContext context,
        IClientRepository userRepository,
        IEmployeeRepository employeeRepository,
        IItemRepository itemRepository,
        IItemGroupRepository itemGroupRepository,
        IWarehouseItemRepository warehouseItemRepository,
        IWarehouseRepository warehouseRepository,
        IInventoryRepository inventories,
        IInventoryItemRepository inventoryItems,
        ITransferRequestRepository transferRequests,
        ITransferRequestItemRepository transferRequestItems,
        ISalesOrderRepository salesOrders,
        ISalesOrderItemRepository salesOrderItems,
        IPaymentRepository payments
        )
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));

        // Repositories
        Clients = userRepository;
        Employees = employeeRepository;

        Items = itemRepository;
        ItemGroups = itemGroupRepository;

        WarehouseItems = warehouseItemRepository;
        Warehouses = warehouseRepository;

        Inventories = inventories;
        InventoryItems = inventoryItems;

        TransferRequestItems = transferRequestItems;
        TransferRequests = transferRequests;

        SalesOrderItems = salesOrderItems;
        SalesOrders = salesOrders;

        Payments = payments;
    }

    #region Repositories
    // First-tier
    public IClientRepository Clients { get; }
    public IEmployeeRepository Employees { get; }

    public IInventoryRepository Inventories { get; }
    public IInventoryItemRepository InventoryItems { get; }

    // Second-tier
    public IItemRepository Items { get; }
    public IItemGroupRepository ItemGroups { get; }

    public IWarehouseItemRepository WarehouseItems { get; }
    public IWarehouseRepository Warehouses { get; }

    public ITransferRequestRepository TransferRequests { get; }
    public ITransferRequestItemRepository TransferRequestItems { get; }

    public ISalesOrderRepository SalesOrders { get; }
    public ISalesOrderItemRepository SalesOrderItems { get; }

    public IPaymentRepository Payments { get; }
    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken);

    #region Transaction Management
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (HasActiveTransaction)
            return;

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
            throw new InvalidOperationException("No active transaction to commit.");
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
            await DisposeCurrentTransactionAsync();
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
            return;

        await _currentTransaction.RollbackAsync(cancellationToken);
        await DisposeCurrentTransactionAsync();
    }

    private async Task DisposeCurrentTransactionAsync()
    {
        await _currentTransaction!.DisposeAsync();
        _currentTransaction = null;
    }
    #endregion

    #region Disposal
    public void Dispose()
    {
        if (_disposed) return;

        _dbContext.Dispose();
        _currentTransaction?.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        await _dbContext.DisposeAsync();
        if (HasActiveTransaction)
            await _currentTransaction!.DisposeAsync();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
    #endregion
}