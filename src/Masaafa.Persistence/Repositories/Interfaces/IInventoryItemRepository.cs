using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IInventoryItemRepository
{
    IQueryable<InventoryItem> Get();

    Task<PaginationResult<InventoryItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<InventoryItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<InventoryItem> CreateAsync(InventoryItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<InventoryItem> UpdateAsync(InventoryItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<InventoryItem> DeleteAsync(InventoryItem item, bool saveChanges = false, CancellationToken cancellationToken = default);
}