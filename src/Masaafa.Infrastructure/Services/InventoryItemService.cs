using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using System.Net;

namespace Masaafa.Infrastructure.Services;

public class InventoryItemService(
    IUnitOfWork unitOfWork,
    IUserContext userContext
    ) : IInventoryItemService
{
    public async Task<PaginationResult<InventoryItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.InventoryItems.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        result.Data = GetSystemQuantityAsync(result.Data);

        return result;
    }

    public async Task<InventoryItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.InventoryItems.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(InventoryItem), nameof(InventoryItem.Id), id.ToString());

        entity.SystemQuantity = entity.WarehouseItem.Quantity;

        return entity;
    }

    public async Task<InventoryItem> CreateAsync(InventoryItem item, CancellationToken cancellationToken = default)
    {
        item.CountedDate = DateTimeOffset.UtcNow;
        item.CountedByUserId = userContext.GetRequiredUserId();

        var warehouseItem = await unitOfWork.WarehouseItems.GetByIdAsync(item.WarehouseItemId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), item.WarehouseItemId.ToString());

        if (item.ActualQuantity > warehouseItem.Quantity)
            throw new CustomException("Actual quantity cant be greater then the available quantity.", HttpStatusCode.BadRequest);

        if (item.ActualQuantity < warehouseItem.Quantity * 90 / 100 && item.Description is null)
            throw new CustomException("Difference between actual quantity and available quantity is high, description is required.", HttpStatusCode.BadRequest);

        var entity = await unitOfWork.InventoryItems.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<InventoryItem> UpdateAsync(Guid id, InventoryItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.InventoryItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(InventoryItem), nameof(InventoryItem.Id), id.ToString());

        if (item.ActualQuantity > exist.WarehouseItem.Quantity)
            throw new CustomException("Actual quantity cant be greater then the available quantity.", HttpStatusCode.BadRequest);

        if (item.ActualQuantity < exist.WarehouseItem.Quantity * 90 / 100 && item.Description is null)
            throw new CustomException("Difference between actual quantity and available quantity is high, description is required.", HttpStatusCode.BadRequest);


        exist.WarehouseItemId = item.WarehouseItemId;
        exist.Notes = item.Notes;
        exist.Description = item.Description;
        exist.ActualQuantity = item.ActualQuantity;

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await GetByIdAsync(id, cancellationToken);

        await unitOfWork.InventoryItems.DeleteAsync(exist);

        return true;

    }

    private IEnumerable<InventoryItem> GetSystemQuantityAsync(IEnumerable<InventoryItem> items)
    {
        var itemList = items.ToList();

        foreach (var item in itemList)
        {
            item.SystemQuantity = item.WarehouseItem.Quantity;
        }

        return itemList;
    }
}