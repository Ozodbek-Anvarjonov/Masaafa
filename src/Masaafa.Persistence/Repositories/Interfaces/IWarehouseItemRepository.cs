using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IWarehouseItemRepository
{
    IQueryable<WarehouseItem> Get();

    Task<PaginationResult<WarehouseItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<PaginationResult<WarehouseItem>> GetByItemIdAsync(
        Guid itemId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<PaginationResult<WarehouseItem>> GetByWarehouseIdAsync(
        Guid warehouseId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<WarehouseItem?> GetByWarehouseIdAndItemIdAsync(Guid warehouseId, Guid itemId, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<WarehouseItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<WarehouseItem> CreateAsync(WarehouseItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<WarehouseItem> UpdateAsync(WarehouseItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<WarehouseItem> DeleteAsync(WarehouseItem item, bool saveChanges = false, CancellationToken cancellationToken = default);
}