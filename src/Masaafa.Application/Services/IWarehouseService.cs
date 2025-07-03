using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IWarehouseService
{
    Task<PaginationResult<Warehouse>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<Warehouse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Warehouse> CreateAsync(Warehouse warehouse, CancellationToken cancellationToken = default);

    Task<Warehouse> UpdateAsync(Guid id, Warehouse warehouse, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}