using Masaafa.Application.Common.Notifications;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Infrastructure.Common.Notifications;

public class PaymentMessageRenderingService : IPaymentMessageRenderingService
{
    public async Task<string> RenderingAsync(Payment payment, PaymentNotificationType type, CancellationToken cancellationToken = default)
    {
        var message = type switch
        {
            PaymentNotificationType.CreatePayment => await CreatePaymentMessage(payment),
            PaymentNotificationType.ApprovePayment => await ApprovePaymentMessage(payment),
            PaymentNotificationType.RejectPayment => await RejectPaymentMessage(payment),
            _ => "Unknown"
        };

        return message;
    }

    private Task<string> CreatePaymentMessage(Payment payment, CancellationToken cancellationToken = default)
    {
        var message = $"{payment.SalesOrder.SalesOrderNumber} raqamli sotuv buyrutmangiz yaratildi.";

        return Task.FromResult(message);
    }

    private Task<string> ApprovePaymentMessage(Payment payment)
    {
        var message = $"{payment.SalesOrder.SalesOrderNumber} raqamli sotuv buyrutmangiz tasdiqlandi.";

        return Task.FromResult(message);
    }

    private Task<string> RejectPaymentMessage(Payment payment)
    {
        var message = $"{payment.SalesOrder.SalesOrderNumber} raqamli sotuv buyrutmangiz rand etildi";

        return Task.FromResult(message);
    }
}