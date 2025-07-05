using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class TransferRequestRepository(AppDbContext context, IUserContext userContext)
    : EntityRepositoryBase<TransferRequest, AppDbContext>(context), ITransferRequestRepository
{
    public async Task<PaginationResult<TransferRequest>> GetAsync(
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
            .Include(entity => entity.FromWarehouse)
            .Include(entity => entity.ToWarehouse)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.ApprovedByUser)
            .Include(entity => entity.RejectedByUser);

        return await exists.ToPaginateAsync(@params, cancellationToken);
    }

    public async Task<TransferRequest?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        exist = exist
            .Include(entity => entity.FromWarehouse)
            .Include(entity => entity.ToWarehouse)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.ApprovedByUser)
            .Include(entity => entity.RejectedByUser);

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<TransferRequest> Get() =>
        base.Get();

    public new Task<TransferRequest> CreateAsync(TransferRequest request, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.CreateAsync(request, saveChanges, cancellationToken);

    public new Task<TransferRequest> UpdateAsync(TransferRequest request, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(request, saveChanges, cancellationToken);

    public new async Task<TransferRequest> DeleteAsync(TransferRequest inventory, bool saveChanges, CancellationToken cancellationToken = default)
    {
        await Context
            .Set<TransferRequestItem>()
            .Where(entity => entity.TransferRequestId == inventory.Id && !entity.IsDeleted)
            .SoftDeleteAsync(userContext.GetRequiredUserId(), cancellationToken);

        return await base.DeleteAsync(inventory, saveChanges, cancellationToken);
    }
}