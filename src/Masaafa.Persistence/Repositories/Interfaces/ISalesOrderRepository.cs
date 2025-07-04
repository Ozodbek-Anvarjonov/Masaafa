using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface ISalesOrderRepository
{
    IQueryable<SalesOrder> Get();

    Task<PaginationResult<SalesOrder>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<SalesOrder?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<SalesOrder> CreateAsync(SalesOrder order, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<SalesOrder> UpdateAsync(SalesOrder order, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<SalesOrder> DeleteAsync(SalesOrder order, bool saveChanges = false, CancellationToken cancellationToken = default);
}