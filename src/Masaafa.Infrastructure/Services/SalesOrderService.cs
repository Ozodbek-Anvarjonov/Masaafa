using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Masaafa.Infrastructure.Services;

public class SalesOrderService(IUnitOfWork unitOfWork, IUserContext userContext, IBalanceService balanceService) : ISalesOrderService
{
    public async Task<PaginationResult<SalesOrder>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.SalesOrders.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<SalesOrder> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrders.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        return exist;
    }

    public async Task<SalesOrder> CreateAsync(SalesOrder order, CancellationToken cancellationToken = default)
    {
        order.CreatedByUserId = userContext.GetRequiredUserId();
        var entity = await unitOfWork.SalesOrders.CreateAsync(order, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<SalesOrder> UpdateAsync(Guid id, SalesOrder order, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrders.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        exist.SalesOrderNumber = order.SalesOrderNumber;
        exist.Note = order.Note;
        exist.DocDate = order.DocDate;
        exist.DocDueDate = order.DocDueDate;
        exist.ClientId = order.ClientId;
        exist.Address = order.Address;
        exist.Latitude = order.Latitude;
        exist.Longitude = order.Longitude;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        await unitOfWork.SalesOrders.DeleteAsync(entity, cancellationToken: cancellationToken);
        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return true;
    }

    public  async Task<SalesOrder> UpdateApproveAsync(Guid id, SalesOrder order, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrders.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        if (order.ConformationStatus is OrderConformationStatus.Approved)
            return exist;


        await unitOfWork.BeginTransactionAsync(cancellationToken);

        exist.ApprovedDate = order.ApprovedDate;
        exist.ApprovedByUserId = userContext.GetRequiredUserId();
        exist.ConformationStatus = OrderConformationStatus.Approved;

        var entities = await unitOfWork.SalesOrderItems
                .Get()
                .Where(entity => entity.SalesOrderId == id && !entity.IsDeleted)
                .Include(entity => entity.WarehouseItem)
                .ToListAsync();

        var payments = await unitOfWork.Payments
            .Get()
            .Where(entity => entity.SalesOrderId == id && !entity.IsDeleted)
            .ExecuteUpdateAsync(call => call.SetProperty(entity => entity.Status, PaymentStatus.InProcess));

        foreach (var entity in entities)
        {
            entity.WarehouseItem.ReservedQuantity += entity.Quantity;
        }

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<SalesOrder> UpdateRejectAsync(Guid id, SalesOrder order, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrders.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        if (exist.ConformationStatus is not OrderConformationStatus.Approved)
        {
            exist.RejectedByUserId = userContext.GetRequiredUserId();
            exist.RejectedDate = order.RejectedDate;
            exist.ConformationStatus = OrderConformationStatus.Rejected;
            exist.RejectionReason = order.RejectionReason;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return exist;
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        if (exist.RejectedDate is not null && exist.Payment?.Status is PaymentStatus.Completed)
            throw new CustomException("Payment is done, before you reject, you must cancel it.", HttpStatusCode.BadRequest);

        var entities = await unitOfWork.SalesOrderItems
                .Get()
                .Where(entity => entity.SalesOrderId == id && !entity.IsDeleted)
                .Include(entity => entity.WarehouseItem)
                .ToListAsync();

        var payments = await unitOfWork.Payments
            .Get()
            .Where(entity => entity.SalesOrderId == id && !entity.IsDeleted)
            .ExecuteUpdateAsync(call => call.SetProperty(entity => entity.Status, PaymentStatus.Rejected));

        foreach (var entity in entities)
        {
            if (entity.ReceivedDate is not null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw new CustomException("Some items is already transferred.", HttpStatusCode.BadRequest);
            }

            entity.WarehouseItem.ReservedQuantity -= entity.Quantity;
        }

        exist.RejectedByUserId = userContext.GetRequiredUserId();
        exist.RejectedDate = order.RejectedDate;
        exist.ConformationStatus = OrderConformationStatus.Rejected;
        exist.RejectionReason = order.RejectionReason;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<SalesOrder> UpdateCancelAsync(Guid id, SalesOrder order, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrders.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        if (exist.IsCancelled)
            return exist;

        if (exist.ConformationStatus is not OrderConformationStatus.Approved)
        {
            exist.CancelledByUserId = userContext.GetRequiredUserId();
            exist.CancelledDate = order.CancelledDate;
            exist.CancellationReason = order.CancellationReason;
            exist.IsCancelled = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return exist;
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var entities = await unitOfWork.SalesOrderItems
                .Get()
                .Where(entity => entity.SalesOrderId == id && !entity.IsDeleted)
                .Include(entity => entity.WarehouseItem)
                .ToListAsync();

        foreach (var entity in entities)
        {
            if (entity.ReceivedDate is not null)
            {
                entity.WarehouseItem.Quantity += entity.Quantity;
            }
            else
                entity.WarehouseItem.ReservedQuantity -= entity.Quantity;
        }

        exist.CancelledByUserId = userContext.GetRequiredUserId();
        exist.CancelledDate = order.CancelledDate;
        exist.CancellationReason = order.CancellationReason;
        exist.IsCancelled = true;

        if (exist.PaymentId is not null && exist.Payment?.Status is PaymentStatus.Completed)
        {
            var payment = await unitOfWork.Payments.GetByIdAsync(exist.PaymentId.Value, false, cancellationToken);
            await balanceService.ReverseBalanceAsync(payment!, entities.Sum(entity => entity.LineTotal), false, cancellationToken);
        }

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }
}