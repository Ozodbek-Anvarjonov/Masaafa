using Masaafa.Application.Common.Notifications;
using Masaafa.Application.Services;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Masaafa.Infrastructure.Common.Notifications;

public class SalesOrderMessageRenderingService(IUnitOfWork unitOfWork, IWarehouseItemService warehouseItemService) : ISalesOrderMessageRenderingService
{
    public async Task<string> RenderingAsync(SalesOrder salesOrder, SalesOrderNotificationType type, CancellationToken cancellationToken = default)
    {
        var message = type switch
        {
            SalesOrderNotificationType.CreateSalesOrder => await CreateSalesOrder(salesOrder, cancellationToken),
            SalesOrderNotificationType.ApproveSalesOrder => await ApproveSalesOrder(salesOrder, cancellationToken),
            SalesOrderNotificationType.RejectSalesOrder => await RejectSalesOrder(salesOrder, cancellationToken),
            SalesOrderNotificationType.CancelSalesOrder => await CancelSalesOrder(salesOrder, cancellationToken),
            SalesOrderNotificationType.SentSalesOrder => await SentSalesOrder(salesOrder, cancellationToken),
            SalesOrderNotificationType.ReceiveSalesOrder => await ReceiveSalesOrder(salesOrder, cancellationToken),
            _ => "unknown"
        };

        return message;
    }


    private async Task<string> CreateSalesOrder(SalesOrder order, CancellationToken cancellationToken = default)
    {
        var message = new StringBuilder();

        message.Append($"Tasdiqlaysizmi?\n" +
            $"Mijoz {order.Client.CardCode}\n");

        var items = await unitOfWork.SalesOrderItems.Get()
            .Where(entity => entity.SalesOrderId == order.Id && !entity.IsDeleted)
            .Select(entity => new {
                ItemName = entity.WarehouseItem.Item.ItemName,
                Quantity = entity.Quantity,
                Price = entity.UnitPrice,
                Discount = entity.DiscountPercent,
            }).ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            message.Append($"{item.ItemName}: Soni - {item.Quantity}, Narxi - {item.Price}, Chegirma - {item.Discount}");
        }

        message.Append($"\nUmumiy miqdori: {items.Sum(entity => entity.Quantity)}\n" +
            $"Umumiy summa: {items.Sum(entity => entity.Price * entity.Quantity * (1 - entity.Discount))}");

        return message.ToString();
    }

    private Task<string> ApproveSalesOrder(SalesOrder order, CancellationToken cancellationToken = default)
    {
        var message = $"{order.SalesOrderNumber} raqamli sotuv buyurutmasi tasdiqlandi.";

        return Task.FromResult(message);
    }

    private Task<string> RejectSalesOrder(SalesOrder order, CancellationToken cancellationToken = default)
    {
        var message = $"{order.SalesOrderNumber} raqamli sotuv buyurutmasi rad etildi.";

        return Task.FromResult(message);
    }

    private Task<string> CancelSalesOrder(SalesOrder order, CancellationToken cancellationToken = default)
    {
        var message = $"{order.SalesOrderNumber} raqamli sotuv buyurutmasi bekor qilindi.";

        return Task.FromResult(message);
    }

    private Task<string> SentSalesOrder(SalesOrder order, CancellationToken cancellationToken = default)
    {
        var message = $"{order.SalesOrderNumber} raqamli sotuv buyurutmasi yuborildi.";

        return Task.FromResult(message);
    }

    private Task<string> ReceiveSalesOrder(SalesOrder order, CancellationToken cancellationToken = default)
    {
        var message = $"{order.SalesOrderNumber} raqamli sotuv buyurutmasi yetkazib berildi.";

        return Task.FromResult(message);
    }

    public async Task<string> ReceiveSalesOrder(SalesOrderItem orderItem, CancellationToken cancellationToken = default)
    {
        var warehouse = await warehouseItemService.GetByIdAsync(orderItem.WarehouseItemId, cancellationToken);

        var message = $"{orderItem.SalesOrder.SalesOrderNumber} raqamli sotuv buyurutmasining {warehouse.Item.ItemName} nomli mahsuloti narxi o'zgardi.";

        return message;
    }
}