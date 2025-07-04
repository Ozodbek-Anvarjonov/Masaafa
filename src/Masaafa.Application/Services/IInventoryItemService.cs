using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IInventoryItemService
{
    Task<PaginationResult<InventoryItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<InventoryItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<InventoryItem> CreateAsync(InventoryItem item, CancellationToken cancellationToken = default);

    Task<InventoryItem> UpdateAsync(Guid id, InventoryItem item, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}