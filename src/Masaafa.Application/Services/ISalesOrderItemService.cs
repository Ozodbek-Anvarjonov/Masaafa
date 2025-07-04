using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface ISalesOrderItemService
{
    Task<PaginationResult<SalesOrderItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<SalesOrderItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SalesOrderItem> CreateAsync(SalesOrderItem item, CancellationToken cancellationToken = default);

    Task<SalesOrderItem> UpdateAsync(Guid id, SalesOrderItem item, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}