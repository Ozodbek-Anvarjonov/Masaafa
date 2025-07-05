using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class PaymentService(IUnitOfWork unitOfWork, IUserContext userContext) : IPaymentService
{
    public async Task<PaginationResult<Payment>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.Payments.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Payments.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Payment), nameof(Payment.Id), id.ToString());

        return exist;
    }

    public async Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        payment.CreatedByUserId = userContext.GetRequiredUserId();
        payment.CreatedDate = DateTimeOffset.UtcNow;
        var entity = await unitOfWork.Payments.CreateAsync(payment, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<Payment> UpdateAsync(Guid id, Payment payment, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Payments.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Payment), nameof(Payment.Id), id.ToString());

        exist.ClientId = payment.ClientId;
        exist.Note = payment.Note;
        exist.SalesOrderId = payment.SalesOrderId;
        exist.Type = payment.Type;
        exist.ClientId = payment.ClientId;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        await unitOfWork.Payments.DeleteAsync(entity, cancellationToken: cancellationToken);

        return true;
    }
}