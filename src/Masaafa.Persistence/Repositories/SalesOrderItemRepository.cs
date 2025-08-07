using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class SalesOrderItemRepository(AppDbContext context)
    : EntityRepositoryBase<SalesOrderItem, AppDbContext>(context), ISalesOrderItemRepository
{
    public async Task<PaginationResult<SalesOrderItem>> GetAsync(
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
            exists = exists.Where(entity => entity.DiscountPercent.ToString().Contains(search)
                || entity.Quantity.ToString().Contains(search)
                || entity.UnitPrice.ToString().Contains(search)
                || entity.SalesOrder.SalesOrderNumber.ToLower().Contains(search.ToLower())
                || entity.SalesOrder.Address.ToLower().Contains(search.ToLower())
                || entity.SalesOrder.SalesOrderNumber.ToLower().Contains(search.ToLower())
                || entity.SalesOrder.Address.ToLower().Contains(search.ToLower())
                || entity.SalesOrder.Client.FirstName.ToLower().Contains(search.ToLower())
                || entity.SalesOrder.Client.LastName.ToLower().Contains(search.ToLower())
                || entity.SalesOrder.Client.PhoneNumber.ToLower().Contains(search.ToLower())
                || entity.SalesOrder.Client.CardCode.ToLower().Contains(search.ToLower()));

        exists = exists
            .OrderBy(filter)
            .Include(entity => entity.SalesOrder)
            .Include(entity => entity.WarehouseItem);

        return await exists.ToPaginateAsync(@params, cancellationToken);
    }

    public async Task<SalesOrderItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        exist = exist
            .Include(entity => entity.SalesOrder)
            .Include(entity => entity.WarehouseItem);

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<SalesOrderItem> Get() =>
        base.Get();

    public new Task<SalesOrderItem> CreateAsync(SalesOrderItem item, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.CreateAsync(item, saveChanges, cancellationToken);

    public new Task<SalesOrderItem> UpdateAsync(SalesOrderItem item, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(item, saveChanges, cancellationToken);

    public new Task<SalesOrderItem> DeleteAsync(SalesOrderItem item, bool saveChanges, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(item, saveChanges, cancellationToken);
    }
}