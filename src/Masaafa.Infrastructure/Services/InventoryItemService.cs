using Masaafa.Application.Services;
using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        result.Data = await GetSystemQuantityAsync(result.Data);

        return result;
    }

    public async Task<InventoryItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.InventoryItems.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(InventoryItem), nameof(InventoryItem.Id), id.ToString());

        entity.SystemQuantity = (await GetWarehouseItemByWarehouseIdAsync(entity.Inventory.WarehouseId, entity.ItemId)).Quantity;

        return entity;
    }

    public async Task<InventoryItem> CreateAsync(InventoryItem item, CancellationToken cancellationToken = default)
    {
        await EnsureWarehouseItemExists(item.InventoryId, item.ItemId, cancellationToken);
        item.CountedDate = DateTimeOffset.UtcNow;
        item.CountedByUserId = userContext.GetRequiredUserId();

        var entity = await unitOfWork.InventoryItems.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<InventoryItem> UpdateAsync(Guid id, InventoryItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.InventoryItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(InventoryItem), nameof(InventoryItem.Id), id.ToString());

        exist.Notes = item.Notes;

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await GetByIdAsync(id, cancellationToken);

        await unitOfWork.InventoryItems.DeleteAsync(exist);

        return true;

    }

    private async Task<IEnumerable<InventoryItem>> GetSystemQuantityAsync(IEnumerable<InventoryItem> items)
    {
        var itemList = items.ToList();

        var keyList = itemList
            .Select(i => new { i.Inventory.WarehouseId, i.ItemId })
            .Distinct()
            .ToList();

        var warehouseIds = keyList.Select(k => k.WarehouseId).Distinct().ToList();
        var itemIds = keyList.Select(k => k.ItemId).Distinct().ToList();

        var warehouseItems = await unitOfWork.WarehouseItems
            .Get()
            .Where(wi => warehouseIds.Contains(wi.WarehouseId) && itemIds.Contains(wi.ItemId))
            .ToListAsync();

        var warehouseItemDict = warehouseItems.ToDictionary(
            wi => (wi.WarehouseId, wi.ItemId),
            wi => wi.Quantity);

        foreach (var item in itemList)
        {
            var key = (item.Inventory.WarehouseId, item.ItemId);
            item.SystemQuantity = warehouseItemDict.TryGetValue(key, out var qty) ? qty : 0;
        }

        return itemList;
    }

    private async Task<WarehouseItem> GetWarehouseItemByWarehouseIdAsync(Guid warehouseId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = await unitOfWork.WarehouseItems.GetByWarehouseIdAndItemIdAsync(warehouseId, itemId)
            ?? throw new NotFoundException($"The warehouse is not exists with Item ID {itemId} and Warehouse Id {warehouseId}");

        return item;
    }

    private async Task EnsureWarehouseItemExists(Guid inventoryId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var inventory = await unitOfWork.Inventories.GetByIdAsync(inventoryId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Inventory), nameof(Inventory.Id), inventoryId.ToString());

        var item = await unitOfWork.WarehouseItems.GetByWarehouseIdAndItemIdAsync(inventory.WarehouseId, itemId)
            ?? throw new NotFoundException($"The warehouse is not exists with Item ID {itemId} and Warehouse Id {inventory.WarehouseId}");
    }
}