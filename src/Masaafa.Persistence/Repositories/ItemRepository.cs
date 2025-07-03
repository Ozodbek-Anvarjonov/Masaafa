using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class ItemRepository(AppDbContext context) : EntityRepositoryBase<Item, AppDbContext>(context), IItemRepository
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

    public new Task<Item> DeleteAsync(Item item, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(item, saveChanges, cancellationToken);
}