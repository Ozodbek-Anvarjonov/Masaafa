using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Items;
using Masaafa.WebApi.Models.Warehouses;

namespace Masaafa.WebApi.Models.SalesOrders;

public class SalesOrderItemResponse
{
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public Guid SalesOrderId { get; set; }
    public SalesOrderResponse SalesOrder { get; set; } = default!;

    public Guid WarehouseItemId { get; set; }
    public WarehouseItem Item { get; set; } = default!;

    public decimal DiscountPercent { get; set; } = 0;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity * (1 - DiscountPercent / 100);

    public DateTimeOffset? SentDate { get; set; }
    public Guid? SendByUserId { get; set; }
    public Employee? SendByUser { get; set; }

    public DateTimeOffset? ReceivedDate { get; set; }
    public Guid? ReceivedByUserId { get; set; }
    public Employee? ReceivedByUser { get; set; }
}