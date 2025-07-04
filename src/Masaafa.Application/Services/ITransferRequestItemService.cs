using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface ITransferRequestItemService
{
    Task<PaginationResult<TransferRequestItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<TransferRequestItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TransferRequestItem> CreateAsync(TransferRequestItem item, CancellationToken cancellationToken = default);

    Task<TransferRequestItem> UpdateAsync(Guid id, TransferRequestItem item, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}