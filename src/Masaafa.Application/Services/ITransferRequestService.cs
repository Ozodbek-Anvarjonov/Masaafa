using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface ITransferRequestService
{
    Task<PaginationResult<TransferRequest>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        );

    Task<TransferRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TransferRequest> CreateAsync(TransferRequest request, CancellationToken cancellationToken = default);

    Task<TransferRequest> UpdateAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default);

    Task<TransferRequest> UpdateWarehousesAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default);

    Task<TransferRequest> UpdateApproveAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default);

    Task<TransferRequest> UpdateRejectAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}