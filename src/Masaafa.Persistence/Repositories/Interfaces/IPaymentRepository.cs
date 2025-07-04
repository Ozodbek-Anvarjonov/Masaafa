using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IPaymentRepository
{
    IQueryable<Payment> Get();

    Task<PaginationResult<Payment>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<Payment?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Payment> CreateAsync(Payment payment, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Payment> UpdateAsync(Payment payment, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Payment> DeleteAsync(Payment payment, bool saveChanges = false, CancellationToken cancellationToken = default);
}