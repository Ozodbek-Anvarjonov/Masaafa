using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IPaymentService
{
    Task<PaginationResult<Payment>> GetAsync(
    PaginationParams @params,
    Filter filter,
    string? search = null,
    CancellationToken cancellationToken = default
    );

    Task<Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default);

    Task<Payment> UpdateAsync(Guid id, Payment payment, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}