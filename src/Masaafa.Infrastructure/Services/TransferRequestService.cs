using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class TransferRequestService(
    IUnitOfWork unitOfWork,
    IUserContext userContext
    ) : ITransferRequestService

{
    public async Task<PaginationResult<TransferRequest>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.TransferRequests.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<TransferRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.TransferRequests.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequest), nameof(TransferRequest.Id), id.ToString());

        return entity;
    }

    public async Task<TransferRequest> CreateAsync(TransferRequest request, CancellationToken cancellationToken = default)
    {
        request.CreatedByUserId = userContext.GetRequiredUserId();
        request.CreatedDate = DateTimeOffset.UtcNow;

        if (request.ApprovedDate is not null)
        {
            request.ApprovedByUserId = userContext.GetRequiredUserId();
            request.Status = TransferRequestStatus.Approved;
        }

        if (request.RejectedDate is not null)
        {
            request.RejectedByUserId = userContext.GetRequiredUserId();
            request.Status = TransferRequestStatus.Rejected;
        }

        var entity = await unitOfWork.TransferRequests.CreateAsync(request, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<TransferRequest> UpdateAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequests.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequest), nameof(TransferRequest.Id), id.ToString());

        exist.RequestNumber = request.RequestNumber;
        exist.Note = request.Note;

        if (exist.ApprovedDate is null && request.ApprovedDate is not null || exist.ApprovedDate != request.ApprovedDate)
        {
            exist.ApprovedByUserId = userContext.GetRequiredUserId();
            exist.Status = TransferRequestStatus.Approved;
        }

        if (exist.RejectedDate is null && request.RejectedDate is not null || exist.RejectedDate != request.RejectedDate)
        {
            exist.RejectedByUserId = userContext.GetRequiredUserId();
            exist.Status = TransferRequestStatus.Rejected;
            exist.RejectionReason = request.RejectionReason;
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        if (exist.FromWarehouseId != request.FromWarehouseId || exist.ToWarehouseId != request.ToWarehouseId)
        {
            await unitOfWork.TransferRequestItems
                .Get()
                .Where(entity => entity.TransferRequestId == request.Id && !entity.IsDeleted)
                .SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);
        }

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await GetByIdAsync(id, cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        await unitOfWork.TransferRequests.DeleteAsync(exist, cancellationToken: cancellationToken);
        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return true;
    }
}