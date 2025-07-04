using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface ISalesOrderItemRepository
{
    IQueryable<SalesOrderItem> Get();

    Task<PaginationResult<SalesOrderItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<SalesOrderItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<SalesOrderItem> CreateAsync(SalesOrderItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<SalesOrderItem> UpdateAsync(SalesOrderItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<SalesOrderItem> DeleteAsync(SalesOrderItem item, bool saveChanges = false, CancellationToken cancellationToken = default);
}