using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface ISalesOrderService
{
    Task<PaginationResult<SalesOrder>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<SalesOrder> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SalesOrder> CreateAsync(SalesOrder order, CancellationToken cancellationToken = default);

    Task<SalesOrder> UpdateAsync(Guid id, SalesOrder order, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}