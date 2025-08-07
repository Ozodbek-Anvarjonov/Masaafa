using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class TransferRequestItemRepository(AppDbContext context)
    : EntityRepositoryBase<TransferRequestItem, AppDbContext>(context), ITransferRequestItemRepository
{
    public async Task<PaginationResult<TransferRequestItem>> GetAsync(
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
            exists = exists.Where(entity => entity.Quantity.ToString().Contains(search)
                || entity.UnitPrice.ToString().Contains(search)
                || entity.TransferRequest.RequestNumber.ToLower().Contains(search.ToLower())
                || entity.FromWarehouseItem.Warehouse.Name.ToLower().Contains(search.ToLower())
                || entity.FromWarehouseItem.Warehouse.Code.ToLower().Contains(search.ToLower())
                || entity.FromWarehouseItem.Warehouse.Address.ToLower().Contains(search.ToLower())
                || entity.ToWarehouseItem.Warehouse.Name.ToLower().Contains(search.ToLower())
                || entity.ToWarehouseItem.Warehouse.Code.ToLower().Contains(search.ToLower())
                || entity.ToWarehouseItem.Warehouse.Address.ToLower().Contains(search.ToLower()));

        exists = exists
            .OrderBy(filter)
            .Include(entity => entity.TransferRequest)
            .Include(entity => entity.FromWarehouseItem)
            .Include(entity => entity.ToWarehouseItem);

        return await exists.ToPaginateAsync(@params, cancellationToken);
    }

    public async Task<TransferRequestItem?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        exist = exist
            .Include(entity => entity.TransferRequest)
            .Include(entity => entity.FromWarehouseItem)
            .Include(entity => entity.ToWarehouseItem);

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<TransferRequestItem> Get() =>
        base.Get();

    public new Task<TransferRequestItem> CreateAsync(TransferRequestItem item, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.CreateAsync(item, saveChanges, cancellationToken);

    public new Task<TransferRequestItem> UpdateAsync(TransferRequestItem item, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(item, saveChanges, cancellationToken);

    public new Task<TransferRequestItem> DeleteAsync(TransferRequestItem item, bool saveChanges, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(item, saveChanges, cancellationToken);
    }
}