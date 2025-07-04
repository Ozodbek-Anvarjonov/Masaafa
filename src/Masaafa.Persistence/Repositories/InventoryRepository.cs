using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class InventoryRepository(AppDbContext context, IUserContext userContext)
    : EntityRepositoryBase<Inventory, AppDbContext>(context), IInventoryRepository
{
    public async Task<PaginationResult<Inventory>> GetAsync(
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
            .Include(entity => entity.Warehouse)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.StartedByUser)
            .Include(entity => entity.CompletedByUser);

        return await exists.ToPaginateAsync(@params, cancellationToken);
    }

    public async Task<Inventory?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        exist = exist
            .Include(entity => entity.Warehouse)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.StartedByUser)
            .Include(entity => entity.CompletedByUser);

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<Inventory> Get() =>
        base.Get();

    public new Task<Inventory> CreateAsync(Inventory inventory, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.CreateAsync(inventory, saveChanges, cancellationToken);

    public new Task<Inventory> UpdateAsync(Inventory inventory, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(inventory, saveChanges, cancellationToken);

    public new async Task<Inventory> DeleteAsync(Inventory inventory, bool saveChanges, CancellationToken cancellationToken = default)
    {
        await Context
            .Set<InventoryItem>()
            .Where(entity => entity.InventoryId == inventory.Id && !entity.IsDeleted)
            .SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);

        return await base.DeleteAsync(inventory, saveChanges, cancellationToken);
    }
}