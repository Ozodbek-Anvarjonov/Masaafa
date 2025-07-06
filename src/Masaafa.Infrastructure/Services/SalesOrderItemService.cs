using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using System.Net;

namespace Masaafa.Infrastructure.Services;

public class SalesOrderItemService(IUnitOfWork unitOfWork, IUserContext userContext, IWarehouseItemService warehouseItemService) : ISalesOrderItemService
{
    public async Task<PaginationResult<SalesOrderItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.SalesOrderItems.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<SalesOrderItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrderItems.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrderItem), nameof(SalesOrderItem.Id), id.ToString());

        return exist;
    }

    public async Task<SalesOrderItem> CreateAsync(SalesOrderItem item, CancellationToken cancellationToken = default)
    {
        var warehouse = await warehouseItemService.GetByIdAsync(item.WarehouseItemId, cancellationToken: cancellationToken);

        if (warehouse.Quantity - warehouse.ReservedQuantity < item.Quantity)
            throw new CustomException("The quantity cant be greater then the available quantity", HttpStatusCode.BadRequest);

        var entity = await unitOfWork.SalesOrderItems.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<SalesOrderItem> UpdateAsync(Guid id, SalesOrderItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrderItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrderItem), nameof(SalesOrderItem.Id), id.ToString());

        if (exist.WarehouseItem.Quantity - exist.WarehouseItem.ReservedQuantity < item.Quantity - exist.Quantity)
            throw new CustomException("The quantity cant be greater then the available quantity", HttpStatusCode.BadRequest);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        exist.Note = item.Note;
        exist.DiscountPercent = item.DiscountPercent;
        exist.UnitPrice = item.UnitPrice;

        var difference = 0 - exist.Quantity + item.Quantity;


        if (exist.Quantity == item.Quantity)
        {
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return exist;
        }

        if (exist.SalesOrder.ConformationStatus is not OrderConformationStatus.Approved)
        {
            exist.Quantity = item.Quantity;
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return exist;
        }

        if (exist.ReceivedDate is not null)
        {
            exist.WarehouseItem.Quantity += difference;

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return exist;
        }

        exist.WarehouseItem.ReservedQuantity += difference;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        await unitOfWork.SalesOrderItems.DeleteAsync(entity, saveChanges: true, cancellationToken: cancellationToken);

        return true;
    }

    public async Task<SalesOrderItem> UpdateWarehouseAsync(Guid id, Guid warehouseId, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrderItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrderItem), nameof(SalesOrderItem.Id), id.ToString());

        var warehouseItem = await unitOfWork.WarehouseItems.GetByIdAsync(warehouseId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), warehouseId.ToString());


        await unitOfWork.BeginTransactionAsync(cancellationToken);

        if (exist.SalesOrder.ConformationStatus is not OrderConformationStatus.Approved)
        {
            exist.WarehouseItemId = warehouseId;
            exist.WarehouseItem = warehouseItem;

            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }

        if (exist.ReceivedDate is not null)
        {
            exist.WarehouseItem.Quantity += exist.Quantity;

            exist.WarehouseItemId = warehouseId;
            exist.WarehouseItem = warehouseItem;

            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }

        exist.WarehouseItem.ReservedQuantity -= exist.Quantity;

        exist.WarehouseItemId = warehouseId;
        exist.WarehouseItem = warehouseItem;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<SalesOrderItem> UpdateSendAsync(Guid id, SalesOrderItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrderItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrderItem), nameof(SalesOrderItem.Id), id.ToString());

        if (exist.SalesOrder.ConformationStatus is not OrderConformationStatus.Approved)
            throw new CustomException("Before the operation starts, the operation must be approved.", HttpStatusCode.BadRequest);

        exist.SentDate = item.SentDate;
        exist.SendByUserId = userContext.GetRequiredUserId();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<SalesOrderItem> UpdateReceiveAsync(Guid id, SalesOrderItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.SalesOrderItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrderItem), nameof(SalesOrderItem.Id), id.ToString());

        if (exist.SalesOrder.ConformationStatus is not OrderConformationStatus.Approved)
            throw new CustomException("Before the operation starts, the operation must be approved.", HttpStatusCode.BadRequest);

        if (exist.SentDate is null)
            throw new CustomException("Before enter receive date, it is required to enter sent date.", HttpStatusCode.BadRequest);

        exist.WarehouseItem.ReservedQuantity -= exist.Quantity;
        exist.WarehouseItem.Quantity -= exist.Quantity;

        exist.ReceivedDate = item.ReceivedDate;
        exist.ReceivedByUserId = userContext.GetRequiredUserId();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }
}