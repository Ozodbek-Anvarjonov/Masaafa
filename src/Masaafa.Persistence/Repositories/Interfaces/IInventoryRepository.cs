using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IInventoryRepository
{
    IQueryable<Inventory> Get();

    Task<PaginationResult<Inventory>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<Inventory?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Inventory> CreateAsync(Inventory inventory, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Inventory> UpdateAsync(Inventory inventory, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Inventory> DeleteAsync(Inventory inventory, bool saveChanges = false, CancellationToken cancellationToken = default);
}