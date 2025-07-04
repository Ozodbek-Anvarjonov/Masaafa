using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class InventoryItemRepository(AppDbContext context)
    : EntityRepositoryBase<InventoryItem, AppDbContext>(context), IInventoryItemRepository
{
    public async Task<PaginationResult<InventoryItem>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = Set.Where(entity => !entity.IsDeleted);

        if (asNoTracking)
            exists = exists.AsNoTracking();

        if (search is not null)
            exists = exists.Where(entity => true);

        exists = exists
            .OrderBy(filter)
            .Include(entity => entity.Inventory)
            .Include(entity => entity.Item)
            .Include(entity => entity.CountedByUser);

        return await exists.ToPaginateAsync(@params, cancellationToken);
    }

    public async Task<InventoryItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        exist = exist
            .Include(entity => entity.Inventory)
            .Include(entity => entity.Item)
            .Include(entity => entity.CountedByUser);

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<InventoryItem> Get() =>
        base.Get();

    public new Task<InventoryItem> CreateAsync(InventoryItem item, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.CreateAsync(item, saveChanges, cancellationToken);

    public new Task<InventoryItem> UpdateAsync(InventoryItem item, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(item, saveChanges, cancellationToken);

    public new Task<InventoryItem> DeleteAsync(InventoryItem item, bool saveChanges, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(item, saveChanges, cancellationToken);
    }
}