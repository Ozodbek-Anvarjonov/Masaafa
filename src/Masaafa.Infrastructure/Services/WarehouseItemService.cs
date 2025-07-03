using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class WarehouseItemService(IUnitOfWork unitOfWork) : IWarehouseItemService
{
    public async Task<PaginationResult<WarehouseItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.WarehouseItems.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<PaginationResult<WarehouseItem>> GetByItemIdAsync(
        Guid itemId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.WarehouseItems.GetByItemIdAsync(itemId, @params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<PaginationResult<WarehouseItem>> GetByWarehouseIdAsync(
        Guid warehouseId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.WarehouseItems.GetByWarehouseIdAsync(warehouseId, @params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<WarehouseItem> GetByWarehouseIdAndItemIdAsync(Guid warehouseId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.WarehouseItems.GetByWarehouseIdAndItemIdAsync(warehouseId, itemId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), warehouseId.ToString());

        return entity;
    }

    public async Task<WarehouseItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.WarehouseItems.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), id.ToString());

        return entity;
    }

    public Task<WarehouseItem> CreateAsync(WarehouseItem item, CancellationToken cancellationToken = default)
    {
        var entity = unitOfWork.WarehouseItems.CreateAsync(item, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<WarehouseItem> UpdateAsync(Guid id, WarehouseItem item, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.WarehouseItems.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(WarehouseItem), nameof(WarehouseItem.Id), id.ToString());

        exist.Quantity = item.Quantity;
        exist.ReservedQuantity = item.ReservedQuantity;

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        _ = await unitOfWork.WarehouseItems.DeleteAsync(entity, true, cancellationToken: cancellationToken);

        return true;
    }
}