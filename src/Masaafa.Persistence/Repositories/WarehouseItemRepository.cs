using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class WarehouseItemRepository(AppDbContext context) : EntityRepositoryBase<WarehouseItem, AppDbContext>(context), IWarehouseItemRepository
{
    public async Task<PaginationResult<WarehouseItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exits = Set.Where(entity => !entity.IsDeleted);

        if (search is not null)
            exits = exits.Where(entity => true);

        exits = exits.OrderBy(filter);
        exits = exits
            .Include(entity => entity.Warehouse)
            .Include(entity => entity.Item);

        if (asNoTracking)
            exits = exits.AsNoTracking();

        return await exits.ToPaginateAsync(@params);
    }

    public async Task<PaginationResult<WarehouseItem>> GetByItemIdAsync(
        Guid itemId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var exits = Set.Where(entity => entity.ItemId == itemId && !entity.IsDeleted);

        if (search is not null)
            exits = exits.Where(entity => true);

        exits = exits.OrderBy(filter);
        exits = exits
            .Include(entity => entity.Warehouse)
            .Include(entity => entity.Item);

        if (asNoTracking)
            exits = exits.AsNoTracking();

        return await exits.ToPaginateAsync(@params);
    }

    public async Task<PaginationResult<WarehouseItem>> GetByWarehouseIdAsync(
        Guid warehouseId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var exits = Set.Where(entity => entity.WarehouseId == warehouseId && !entity.IsDeleted);

        if (search is not null)
            exits = exits.Where(entity => true);

        exits = exits.OrderBy(filter);
        exits = exits
            .Include(entity => entity.Warehouse)
            .Include(entity => entity.Item);

        if (asNoTracking)
            exits = exits.AsNoTracking();

        return await exits.ToPaginateAsync(@params);
    }

    public async Task<WarehouseItem?> GetByWarehouseIdAndItemIdAsync(
        Guid warehouseId,
        Guid itemId,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.WarehouseId == warehouseId  && entity.ItemId == itemId && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<WarehouseItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<WarehouseItem> Get() =>
        base.Get();

    public new Task<WarehouseItem> CreateAsync(WarehouseItem item, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.CreateAsync(item, saveChanges, cancellationToken);

    public new Task<WarehouseItem> UpdateAsync(WarehouseItem item, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(item, saveChanges, cancellationToken);

    public new Task<WarehouseItem> DeleteAsync(WarehouseItem item, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(item, saveChanges, cancellationToken);
}