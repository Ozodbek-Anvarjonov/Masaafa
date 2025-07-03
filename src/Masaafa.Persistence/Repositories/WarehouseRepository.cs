using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class WarehouseRepository(AppDbContext context, IUserContext userContext) : EntityRepositoryBase<Warehouse, AppDbContext>(context), IWarehouseRepository
{
    public async Task<PaginationResult<Warehouse>> GetAsync(
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

        if (asNoTracking)
            exits = exits.AsNoTracking();

        return await exits.ToPaginateAsync(@params);
    }

    public Task<Warehouse?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<Warehouse> Get() =>
        base.Get();

    public new Task<Warehouse> CreateAsync(Warehouse warehouse, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.CreateAsync(warehouse, saveChanges, cancellationToken);

    public new Task<Warehouse> UpdateAsync(Warehouse warehouse, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(warehouse, saveChanges, cancellationToken);

    public new async Task<Warehouse> DeleteAsync(Warehouse warehouse, bool saveChanges = false, CancellationToken cancellationToken = default)
    {
        await Context
            .Set<WarehouseItem>()
            .Where(entity => entity.WarehouseId == warehouse.Id)
            .SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);

        return await base.DeleteAsync(warehouse, saveChanges, cancellationToken);
    }
}