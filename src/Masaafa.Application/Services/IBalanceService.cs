using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IBalanceService
{
    public Task BalanceAsync(Payment payment, bool saveChanges = true, CancellationToken cancellationToken = default);

    public Task ReverseBalanceAsync(Payment payment, decimal amount, bool saveChanges = true, CancellationToken cancellationToken = default);
}