using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class PaymentRepository(AppDbContext context)
    : EntityRepositoryBase<Payment, AppDbContext>(context), IPaymentRepository
{
    public async Task<PaginationResult<Payment>> GetAsync(
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
            .Include(entity => entity.SalesOrder)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.CompletedByUser);

        return await exists.ToPaginateAsync(@params, cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        exist = exist
            .Include(entity => entity.Client)
            .Include(entity => entity.SalesOrder)
            .Include(entity => entity.CreatedByUser)
            .Include(entity => entity.CompletedByUser);

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<Payment> Get() =>
        base.Get();

    public new Task<Payment> CreateAsync(Payment payment, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.CreateAsync(payment, saveChanges, cancellationToken);

    public new Task<Payment> UpdateAsync(Payment payment, bool saveChanges, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(payment, saveChanges, cancellationToken);

    public new Task<Payment> DeleteAsync(Payment payment, bool saveChanges, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(payment, saveChanges, cancellationToken);
    }
}