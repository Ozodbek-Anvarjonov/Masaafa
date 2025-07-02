using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class SalesOrderItem : SoftDeletedEntity
{
    public Guid SalesOrderId { get; set; }
    public SalesOrder SalesOrder { get; set; } = default!;

    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal DiscountPercent { get; set; } = 0;

    public Guid WarehouseId { get; set; }
    public WareHouse Warehouse { get; set; } = default!;

    public decimal LineTotal => UnitPrice * Quantity * (1 - DiscountPercent / 100);

    public string? Notes { get; set; }
    public bool IsCancelled { get; set; } = false;
    public DateTime? CancelledDate { get; set; }
    public string? CancellationReason { get; set; }
}