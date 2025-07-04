using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IInventoryService
{
    Task<PaginationResult<Inventory>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<Inventory> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Inventory> CreateAsync(Inventory inventory, CancellationToken cancellationToken = default);

    Task<Inventory> UpdateAsync(Guid id, Inventory inventory, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}