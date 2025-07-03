using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class ItemRepository(AppDbContext context, IUserContext userContext) : EntityRepositoryBase<Item, AppDbContext>(context), IItemRepository
{
    public async Task<PaginationResult<Item>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = Set.Where(entity => !entity.IsDeleted);

        if (search is not null)
            exists = exists.Where(entity => true);

        exists = exists.OrderBy(filter);
        exists = exists.Include(entity => entity.ItemGroup);

        if (asNoTracking)
            exists = exists.AsNoTracking();

        return await exists.ToPaginateAsync(@params);
    }

    public async Task<PaginationResult<Item>> GetByGroupIdAsync(
        Guid groupId,
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = Set.Where(entity => entity.ItemGroupId == groupId && !entity.IsDeleted);

        if (search is not null)
            exists = exists.Where(entity => true);

        exists = exists.OrderBy(filter);
        exists = exists.Include(entity => entity.ItemGroup);

        if (asNoTracking)
            exists = exists.AsNoTracking();

        return await exists.ToPaginateAsync(@params);
    }

    public Task<Item?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        exist = exist.Include(entity => entity.ItemGroup);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<Item> Get() =>
        base.Get();

    public new Task<Item> CreateAsync(Item item, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.CreateAsync(item, saveChanges, cancellationToken);

    public new Task<Item> UpdateAsync(Item item, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(item, saveChanges, cancellationToken);

    public new async Task<Item> DeleteAsync(Item item, bool saveChanges = false, CancellationToken cancellationToken = default)
    {
        await Context
            .Set<WarehouseItem>()
            .Where(entity => entity.WarehouseId == item.Id)
            .SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);

        return await base.DeleteAsync(item, saveChanges, cancellationToken);
    }
}