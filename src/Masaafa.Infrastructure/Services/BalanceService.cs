using Masaafa.Application.Services;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class BalanceService(IUnitOfWork unitOfWork) : IBalanceService
{
    public async Task BalanceAsync(Payment payment, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var amount = unitOfWork.SalesOrderItems
            .Get()
            .Where(entity => entity.SalesOrderId == payment.SalesOrderId && !entity.IsDeleted)
            .Sum(entity => entity.Quantity * entity.UnitPrice * (1 - entity.DiscountPercent / 100));

        if (payment.Type is PaymentType.Incoming)
            payment.Client.Balance -= amount;
        else
            payment.Client.Balance += amount;

        if (saveChanges)
            await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ReverseBalanceAsync(Payment payment, decimal amount, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (payment.Type is PaymentType.Incoming)
            payment.Client.Balance += amount;
        else
            payment.Client.Balance -= amount;

        if (saveChanges)
            await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}