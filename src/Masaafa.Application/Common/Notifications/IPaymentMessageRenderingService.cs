using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Application.Common.Notifications;

public interface IPaymentMessageRenderingService
{
    Task<string> RenderingAsync(Payment payment, PaymentNotificationType type, CancellationToken cancellationToken = default);
}