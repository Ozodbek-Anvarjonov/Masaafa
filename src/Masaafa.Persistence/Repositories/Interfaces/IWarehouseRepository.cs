using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IWarehouseRepository
{
    IQueryable<Warehouse> Get();

    Task<PaginationResult<Warehouse>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<Warehouse?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Warehouse> CreateAsync(Warehouse warehouse, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Warehouse> UpdateAsync(Warehouse warehouse, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Warehouse> DeleteAsync(Warehouse warehouse, bool saveChanges = false, CancellationToken cancellationToken = default);
}