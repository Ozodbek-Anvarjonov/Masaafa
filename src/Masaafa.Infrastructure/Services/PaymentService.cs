using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using System.Net;

namespace Masaafa.Infrastructure.Services;

public class PaymentService(
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    ISalesOrderService salesOrderService,
    IBalanceService balanceService) : IPaymentService
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
        var order = await unitOfWork.SalesOrders.GetByIdAsync(payment.SalesOrderId, false, cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), payment.SalesOrderId.ToString());

        if (order.ConformationStatus is OrderConformationStatus.Rejected)
            throw new CustomException("It is not possible to create payment for rejected orders.", HttpStatusCode.BadRequest);

        payment.CreatedByUserId = userContext.GetRequiredUserId();
        payment.CreatedDate = DateTimeOffset.UtcNow;
        var entity = await unitOfWork.Payments.CreateAsync(payment, saveChanges: true, cancellationToken: cancellationToken);

        order.PaymentId = entity.Id;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Payment> UpdateAsync(Guid id, Payment payment, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Payments.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Payment), nameof(Payment.Id), id.ToString());

        var order = await unitOfWork.SalesOrders.GetByIdAsync(payment.SalesOrderId, false, cancellationToken)
                ?? throw new NotFoundException(nameof(SalesOrder), nameof(SalesOrder.Id), id.ToString());

        if (order.ConformationStatus is OrderConformationStatus.Rejected)
            throw new CustomException("It is not possible to create payment for rejected orders.", HttpStatusCode.BadRequest);

        exist.Note = payment.Note;
        exist.Type = payment.Type;
        exist.ClientId = payment.ClientId;

        if (exist.SalesOrderId != payment.SalesOrderId)
        {
            exist.SalesOrder.PaymentId = null;
            exist.SalesOrder.Payment = null;

            order.PaymentId = exist.Id;
            order.Payment = exist;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        await unitOfWork.Payments.DeleteAsync(entity, cancellationToken: cancellationToken);

        return true;
    }

    public async Task<Payment> UpdateCompleteAsync(Guid id, Payment payment, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var exist = await unitOfWork.Payments.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Payment), nameof(Payment.Id), id.ToString());

        if (exist.SalesOrder.ConformationStatus is OrderConformationStatus.Rejected)
        {
            exist.Status = PaymentStatus.Rejected;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new CustomException("The order is already rejected.", HttpStatusCode.BadRequest);
        }

        if (exist.SalesOrder.ConformationStatus is not OrderConformationStatus.Approved)
            throw new CustomException("The order is not approved yet.", HttpStatusCode.BadRequest);

        await balanceService.BalanceAsync(exist, saveChanges: false, cancellationToken);

        exist.CompletedByUserId = userContext.GetRequiredUserId();
        exist.CompletedDate = payment.CompletedDate;
        exist.Status = PaymentStatus.Completed;

        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return exist;
    }
}