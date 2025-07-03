using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IWarehouseItemService
{
    Task<PaginationResult<WarehouseItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<WarehouseItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<WarehouseItem> CreateAsync(WarehouseItem item, CancellationToken cancellationToken = default);

    Task<WarehouseItem> UpdateAsync(Guid id, WarehouseItem item, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}