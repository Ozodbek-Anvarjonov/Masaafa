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
        var warehouseItems = await EnsureWarehouseItemExists(item.FromWarehouseItemId, item.ToWarehouseItemId);

        if  (warehouseItems.From.Quantity - warehouseItems.From.ReservedQuantity < item.Quantity)
            throw new CustomException("The quantity cant be greater then the available quantity", HttpStatusCode.BadRequest);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var entity = await unitOfWork.TransferRequestItems.CreateAsync(item, saveChanges: false, cancellationToken: cancellationToken);

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return entity;
    }

    public async Task<TransferRequestItem> UpdateAsync(Guid id, TransferRequestItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequestItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequestItem), nameof(TransferRequestItem.Id), id.ToString());

        if (exist.FromWarehouseItem.Quantity - exist.FromWarehouseItem.ReservedQuantity < item.Quantity - exist.Quantity)
            throw new CustomException("The quantity cant be greater then the available quantity", HttpStatusCode.BadRequest);


        await unitOfWork.BeginTransactionAsync(cancellationToken);

        exist.Note = item.Note;
        exist.UnitPrice = item.UnitPrice;

        var difference = 0 - exist.Quantity + item.Quantity;
        if (exist.Quantity == item.Quantity)
        {
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return exist;
        }

        if (exist.TransferRequest.Status != TransferRequestStatus.Approved)
        {
            exist.Quantity = item.Quantity;
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return exist;
        }

        if (exist.ReceivedDate is not null)
        {
            exist.FromWarehouseItem.Quantity -= difference;
            exist.FromWarehouseItem.ReservedQuantity -= difference;
            exist.ToWarehouseItem.Quantity += difference;

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return exist;
        }

        exist.FromWarehouseItem.ReservedQuantity = exist.FromWarehouseItem.ReservedQuantity + difference;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await GetByIdAsync(id, cancellationToken);

        await unitOfWork.TransferRequestItems.DeleteAsync(exist, saveChanges: true, cancellationToken: cancellationToken);

        return true;
    }

    public async Task<TransferRequestItem> UpdateWarehouseItemAsync(Guid id, TransferRequestItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequestItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequestItem), nameof(TransferRequestItem.Id), id.ToString());

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var items = await EnsureWarehouseItemExists(item.FromWarehouseItemId, item.ToWarehouseItemId, cancellationToken);

        if (exist.TransferRequest.Status is not TransferRequestStatus.Approved)
        {
            exist.FromWarehouseItemId = item.FromWarehouseItemId;
            exist.ToWarehouseItemId = item.ToWarehouseItemId;
            exist.FromWarehouseItem = items.From;
            exist.ToWarehouseItem = items.To;

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return exist;
        }

        if (exist.ReceivedDate is not null)
        {
            exist.ToWarehouseItem.Quantity -= exist.Quantity;
            exist.FromWarehouseItem.Quantity += exist.Quantity;

            exist.FromWarehouseItemId = item.FromWarehouseItemId;
            exist.ToWarehouseItemId = item.ToWarehouseItemId;
            exist.FromWarehouseItem = items.From;
            exist.ToWarehouseItem = items.To;

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return exist;
        }

        exist.FromWarehouseItem.ReservedQuantity -= exist.Quantity;

        exist.FromWarehouseItemId = item.FromWarehouseItemId;
        exist.ToWarehouseItemId = item.ToWarehouseItemId;
        exist.FromWarehouseItem = items.From;
        exist.ToWarehouseItem = items.To;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }

    public async Task<TransferRequestItem> UpdateSendDateAsync(Guid id, TransferRequestItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequestItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequestItem), nameof(TransferRequestItem.Id), id.ToString());

        if (exist.TransferRequest.Status is not TransferRequestStatus.Approved)
            throw new CustomException("Before the operation starts, the operation must be approved.", HttpStatusCode.BadRequest);

        exist.SentDate = item.SentDate;
        exist.SendByUserId = userContext.GetRequiredUserId();
        exist.TransferRequest.ProcessStatus = TransferProcessStatus.Left;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<TransferRequestItem> UpdateReceiveAsync(Guid id, TransferRequestItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.TransferRequestItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TransferRequestItem), nameof(TransferRequestItem.Id), id.ToString());

        if (exist.TransferRequest.Status is not TransferRequestStatus.Approved)
            throw new CustomException("Before the operation starts, the operation must be approved.", HttpStatusCode.BadRequest);

        if (exist.SentDate is null)
            throw new CustomException("Before enter receive date, it is required to enter sent date.", HttpStatusCode.BadRequest);

        exist.FromWarehouseItem.ReservedQuantity -= exist.Quantity;
        exist.FromWarehouseItem.Quantity = exist.Quantity;
        exist.ToWarehouseItem.Quantity += exist.Quantity;
        
        exist.ReceivedDate = item.ReceivedDate;
        exist.ReceivedByUserId = userContext.GetRequiredUserId();
        exist.TransferRequest.ProcessStatus = TransferProcessStatus.Done;

        await unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return exist;
    }

    private async Task<WarehouseItem> GetWarehouseItemByWarehouseIdAsync(Guid warehouseId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = await unitOfWork.WarehouseItems.GetByWarehouseIdAndItemIdAsync(warehouseId, itemId, asNoTracking: false)
            ?? throw new NotFoundException($"The warehouse is not exists with WarehouseItem ID {itemId} and WarehouseItem Id {warehouseId}");

        return item;
    }

    private async Task<(WarehouseItem From, WarehouseItem To)> EnsureWarehouseItemExists(Guid fromWarehouseItemId, Guid toWarehouseItemId, CancellationToken cancellationToken = default)
    {
        var from = await unitOfWork.WarehouseItems.GetByIdAsync(fromWarehouseItemId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), fromWarehouseItemId.ToString());

        var to = await unitOfWork.WarehouseItems.GetByIdAsync(toWarehouseItemId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), toWarehouseItemId.ToString());

        if (from.ItemId != to.ItemId)
            throw new CustomException("Transfer request item is not same.", HttpStatusCode.BadRequest);

        return new(from, to);
    }
}