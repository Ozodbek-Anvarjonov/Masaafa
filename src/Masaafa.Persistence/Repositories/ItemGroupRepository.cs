using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class ItemGroupRepository(AppDbContext context, IUserContext userContext) : EntityRepositoryBase<ItemGroup, AppDbContext>(context), IItemGroupRepository
{
    public async Task<PaginationResult<ItemGroup>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = Set.Where(entity => !entity.IsDeleted);

        if (search is not null)
            exists = exists.Where(entity => entity.Name.ToLower().Contains(search.ToLower())
                || entity.Description.ToLower().Contains(search.ToLower()));

        exists = exists.OrderBy(filter);

        if (asNoTracking)
            exists = exists.AsNoTracking();

        return await exists.ToPaginateAsync(@params);
    }

    public async Task<ItemGroup?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<ItemGroup> Get() =>
        base.Get();

    public new Task<ItemGroup> CreateAsync(ItemGroup itemGroup, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.CreateAsync(itemGroup, saveChanges, cancellationToken);

    public new Task<ItemGroup> UpdateAsync(ItemGroup itemGroup, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(itemGroup, saveChanges, cancellationToken);

    public new async Task<ItemGroup> DeleteAsync(ItemGroup itemGroup, bool saveChanges = false, CancellationToken cancellationToken = default)
    {
        await Context
            .Set<Item>()
            .Where(entity => entity.ItemGroupId == itemGroup.Id)
            .SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);

        return await base.DeleteAsync(itemGroup, saveChanges, cancellationToken);
    }
}