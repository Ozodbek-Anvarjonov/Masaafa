using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface ITransferRequestRepository
{
    IQueryable<TransferRequest> Get();

    Task<PaginationResult<TransferRequest>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<TransferRequest?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<TransferRequest> CreateAsync(TransferRequest request, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<TransferRequest> UpdateAsync(TransferRequest request, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<TransferRequest> DeleteAsync(TransferRequest request, bool saveChanges = false, CancellationToken cancellationToken = default);
}