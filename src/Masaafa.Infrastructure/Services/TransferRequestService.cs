using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

        var entity = await unitOfWork.TransferRequests.CreateAsync(request, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<TransferRequest> UpdateAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var exist = await unitOfWork.TransferRequests.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequest), nameof(TransferRequest.Id), id.ToString());

        exist.RequestNumber = request.RequestNumber;
        exist.Note = request.Note;

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

    public async Task<TransferRequest> UpdateWarehousesAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequests.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequest), nameof(TransferRequest.Id), id.ToString());

        if (exist.FromWarehouseId == request.FromWarehouseId && exist.ToWarehouseId == request.ToWarehouseId)
            return exist;

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var entities = unitOfWork.TransferRequestItems
                .Get()
                .Where(entity => entity.TransferRequestId == request.Id && !entity.IsDeleted);

        if (exist.Status != TransferRequestStatus.Approved)
        {
            await entities.SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);
            return exist;
        }

        foreach (var entity in entities)
        {
            entity.FromWarehouseItem.ReservedQuantity -= entity.Quantity;
            if (entity.ReceivedDate is not null)
            {
                entity.ToWarehouseItem.Quantity -= entity.Quantity;
                entity.FromWarehouseItem.Quantity += entity.Quantity;
            }

            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;
            entity.DeletedBy = userContext.GetRequiredUserId();
        }

        exist.FromWarehouseId = request.FromWarehouseId;
        exist.ToWarehouseId = request.ToWarehouseId;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<TransferRequest> UpdateApproveAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequests.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequest), nameof(TransferRequest.Id), id.ToString());

        if (exist.Status is TransferRequestStatus.Approved)
            return exist;

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        exist.ApprovedDate = request.ApprovedDate;
        exist.ApprovedByUserId = userContext.GetRequiredUserId();
        exist.Status = TransferRequestStatus.Approved;

        var entities = await unitOfWork.TransferRequestItems
                .Get()
                .Where(entity => entity.TransferRequestId == request.Id && !entity.IsDeleted)
                .Include(entity => entity.FromWarehouseItem)
                .ToListAsync();

        foreach (var entity in entities)
        {
            entity.FromWarehouseItem.ReservedQuantity += entity.Quantity;
        }

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<TransferRequest> UpdateRejectAsync(Guid id, TransferRequest request, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequests.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequest), nameof(TransferRequest.Id), id.ToString());


        if (exist.Status is not TransferRequestStatus.Approved)
        {
            exist.RejectedByUserId = userContext.GetRequiredUserId();
            exist.RejectedDate = request.RejectedDate;
            exist.Status = TransferRequestStatus.Rejected;
            exist.RejectionReason = request.RejectionReason;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return exist;
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var entities = await unitOfWork.TransferRequestItems
                .Get()
                .Where(entity => entity.TransferRequestId == request.Id && !entity.IsDeleted)
                .Include(entity => entity.FromWarehouseItem)
                .ToListAsync();

        foreach (var entity in entities)
        {
            if (entity.ReceivedDate is not null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw new CustomException("Some items is already transferred.", HttpStatusCode.BadRequest);
            }

            entity.FromWarehouseItem.ReservedQuantity -= entity.Quantity;
        }

        exist.RejectedByUserId = userContext.GetRequiredUserId();
        exist.RejectedDate = request.RejectedDate;
        exist.Status = TransferRequestStatus.Rejected;
        exist.RejectionReason = request.RejectionReason;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }
}