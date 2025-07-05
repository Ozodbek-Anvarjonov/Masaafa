using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using System.Net;

namespace Masaafa.Infrastructure.Services;

public class TransferRequestItemService(IUnitOfWork unitOfWork, IUserContext userContext) : ITransferRequestItemService
{
    public async Task<PaginationResult<TransferRequestItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.TransferRequestItems.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<TransferRequestItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequestItems.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequestItem), nameof(TransferRequestItem.Id), id.ToString());

        return exist;
    }

    public async Task<TransferRequestItem> CreateAsync(TransferRequestItem item, CancellationToken cancellationToken = default)
    {
        await EnsureWarehouseItemExists(item.FromWarehouseItemId, item.ToWarehouseItemId);

        if (item.SentDate is not null)
            item.SendByUserId = userContext.GetRequiredUserId();
        if (item.ReceivedDate is not null)
            item.ReceivedByUserId = userContext.GetRequiredUserId();

        var entity = await unitOfWork.TransferRequestItems.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<TransferRequestItem> UpdateAsync(Guid id, TransferRequestItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequestItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequestItem), nameof(TransferRequestItem.Id), id.ToString());

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        if (exist.FromWarehouseItemId != item.FromWarehouseItemId
            && exist.ToWarehouseItemId != item.ToWarehouseItemId)
            await EnsureWarehouseItemExists(item.FromWarehouseItemId, item.ToWarehouseItemId);

        if (exist.SentDate is null && item.SentDate is not null
            && exist.TransferRequest.Status is TransferRequestStatus.Approved)
        {
            exist.SentDate = item.SentDate;
            exist.SendByUserId = userContext.GetRequiredUserId();
        }

        if (exist.ReceivedDate is null && item.ReceivedDate is not null
            && exist.TransferRequest.Status is TransferRequestStatus.Approved)
        {
            exist.ReceivedDate = item.ReceivedDate;
            exist.ReceivedByUserId = userContext.GetRequiredUserId();
        }


        exist.Note = item.Note;
        exist.Quantity = item.Quantity;
        exist.UnitPrice = item.UnitPrice;
        exist.FromWarehouseItem = item.FromWarehouseItem;
        exist.ToWarehouseItem = item.ToWarehouseItem;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await GetByIdAsync(id, cancellationToken);

        await unitOfWork.TransferRequestItems.DeleteAsync(exist, saveChanges: true, cancellationToken: cancellationToken);

        return true;
    }

    private async Task<WarehouseItem> GetWarehouseItemByWarehouseIdAsync(Guid warehouseId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = await unitOfWork.WarehouseItems.GetByWarehouseIdAndItemIdAsync(warehouseId, itemId, asNoTracking: false)
            ?? throw new NotFoundException($"The warehouse is not exists with WarehouseItem ID {itemId} and WarehouseItem Id {warehouseId}");

        return item;
    }

    private async Task EnsureWarehouseItemExists(Guid fromWarehouseItemId, Guid toWarehouseItemId, CancellationToken cancellationToken = default)
    {
        var from = await unitOfWork.WarehouseItems.GetByIdAsync(fromWarehouseItemId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), fromWarehouseItemId.ToString());

        var to = await unitOfWork.WarehouseItems.GetByIdAsync(toWarehouseItemId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), toWarehouseItemId.ToString());

        if (from.ItemId != to.ItemId)
            throw new CustomException("Transfer request item is not same.", HttpStatusCode.BadRequest);
    }
}