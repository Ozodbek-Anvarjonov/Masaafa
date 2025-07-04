using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface ITransferRequestItemRepository
{
    IQueryable<TransferRequestItem> Get();

    Task<PaginationResult<TransferRequestItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<TransferRequestItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<TransferRequestItem> CreateAsync(TransferRequestItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<TransferRequestItem> UpdateAsync(TransferRequestItem item, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<TransferRequestItem> DeleteAsync(TransferRequestItem item, bool saveChanges = false, CancellationToken cancellationToken = default);
}