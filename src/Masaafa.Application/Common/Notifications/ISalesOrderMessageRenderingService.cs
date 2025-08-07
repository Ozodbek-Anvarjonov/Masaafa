using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Application.Common.Notifications;

public interface ISalesOrderMessageRenderingService
{
    Task<string> RenderingAsync(SalesOrder salesOrder, SalesOrderNotificationType type, CancellationToken cancellationToken = default);

    Task<string> ReceiveSalesOrder(SalesOrderItem orderItem, CancellationToken cancellationToken = default);
}