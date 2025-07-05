using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class SalesOrderRepository(AppDbContext context, IUserContext userContext)
    : EntityRepositoryBase<SalesOrder, AppDbContext>(context), ISalesOrderRepository
{
    public async Task<PaginationResult<SalesOrder>> GetAsync(
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
            .Include(entity => entity.Client)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.ApprovedByUser)
            .Include(entity => entity.RejectedByUser)
            .Include(entity => entity.CancelledByUser);

        return await exists.ToPaginateAsync(@params, cancellationToken);
    }

    public async Task<SalesOrder?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        exist = exist
            .Include(entity => entity.Client)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.ApprovedByUser)
            .Include(entity => entity.RejectedByUser)
            .Include(entity => entity.CancelledByUser);

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<SalesOrder> Get() =>
        base.Get();

    public new Task<SalesOrder> CreateAsync(SalesOrder order, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.CreateAsync(order, saveChanges, cancellationToken);

    public new Task<SalesOrder> UpdateAsync(SalesOrder order, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(order, saveChanges, cancellationToken);

    public new async Task<SalesOrder> DeleteAsync(SalesOrder order, bool saveChanges, CancellationToken cancellationToken = default)
    {
        await Context
            .Set<SalesOrderItem>()
            .Where(entity => entity.SalesOrderId == order.Id && !entity.IsDeleted)
            .SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);

        return await base.DeleteAsync(order, saveChanges, cancellationToken);
    }
}